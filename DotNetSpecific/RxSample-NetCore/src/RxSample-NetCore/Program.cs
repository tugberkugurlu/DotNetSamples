using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RxSample_NetCore;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ConsoleApplication
{
    public class Program
    {
        private static readonly int ConcurrencyLevel = Environment.ProcessorCount * 2;

        public static void Main(string[] args)
        {
            var loggerFactory = LoggerFactoryBuilder.Build();
            var logger = loggerFactory.CreateLogger<Program>();
            var cts = new CancellationTokenSource();

            // see for SIGTERM support of coreclr: https://github.com/dotnet/coreclr/issues/2688
            // also see https://github.com/Microsoft/vsts-agent/issues/215
            // also see https://github.com/dotnet/corefx/issues/5205
            // For all UNIX Signals, see https://github.com/dotnet/corefx/issues/3188
            var currentAssembly = typeof(Program).GetTypeInfo().Assembly;
            AssemblyLoadContext.GetLoadContext(currentAssembly).Unloading += (assemblyLoadContext) => 
            {
                logger.LogWarning("Termination requested");
                cts.Cancel();
            };

            RabbitMqObserver(logger, cts.Token).Wait();
        }

        // http://stackoverflow.com/questions/16658915/reactive-extensions-concurrency-within-the-subscriber
        // https://social.msdn.microsoft.com/Forums/en-US/6ef0caba-709d-450a-830d-8fc80f0a815d/the-rx-serialization-guarantee?forum=rx
        private static async Task RabbitMqObserver(ILogger logger, CancellationToken cancellationToken = default(CancellationToken))
        {
            logger.LogInformation("ConcurrencyLevel: {concurrencyLevel}", ConcurrencyLevel);
            Sleep(logger, TimeSpan.FromSeconds(10), "to let RabbitMQ start up");

            var config = ConfigBuilder.Build();
            var settings = new MessageQueueSettings();
            ConfigurationBinder.Bind(config.GetSection("MessageQueue"), settings);

            using(var context = new RabbitContext(settings, logger))
            {
                var publisher = Task.Run(async () => 
                {
                    do
                    {
                        context.Publish(new Bogus.Faker().Lorem.Sentence(20));
                        await Task.Delay(TimeSpan.FromMilliseconds(100));

                    } while (cancellationToken.IsCancellationRequested == false);   
                });

                var subscriber = Task.Run(async () => 
                {
                    // Action to invoke for each element in the observable sequence.
                    Action<IMessage<string>> onNext = message => 
                    {
                        logger.LogDebug("Started handling {messageId}", message.Id);

                        try
                        {
                            Thread.Sleep(3000);
                            Console.WriteLine(message.GetContent());
                            message.CompleteAsync().Wait();
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(0, ex, "Error while handling {messageId}", message.Id);
                            message.AbandonAsync().Wait();
                        }
                    };

                    // Action to invoke upon exceptional termination of the observable sequence
                    Action<Exception> onError = ex => 
                    {
                        logger.LogError(0, ex, "Exceptional termination of the observable sequence");
                    };

                    // Action to invoke upon graceful termination of the observable sequence.
                    Action onCompleted = () => 
                    {
                        logger.LogDebug("Graceful termination of the observable sequence");
                    };

                    using(var subscription = context.CreateSubscription())
                    using(subscription.Observable.ObserveOn(NewThreadScheduler.Default).Subscribe(onNext, onError, onCompleted))
                    {
                        await SleepTillCancelledAsync(cancellationToken);
                    }
                });

                await Task.WhenAll(publisher, subscriber);
            }
        }

        private static void Sleep(ILogger logger, TimeSpan sleepAmount, string message) 
        {
            logger.LogInformation("Waiting for {sleepAmount}: {message}", sleepAmount, message);
            Thread.Sleep(sleepAmount);
        }

        private static async Task SleepTillCancelledAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            }
            catch (TaskCanceledException) { }
        }

        /// <remarks>
        /// http://www.rabbitmq.com/releases/rabbitmq-dotnet-client/v2.8.1/rabbitmq-dotnet-client-2.8.1-user-guide.pdf
        /// https://groups.google.com/forum/#!topic/rabbitmq-users/qQLMY94rdBc
        /// </remarks>
        private class RabbitContext : IDisposable
        {
            private const string ExchangeName = "messages";
            private const string QueueName = "message_processor";
            private const string RoutingKey = "message_received";
            private readonly IConnection _connection;
            private readonly ILogger _logger;

            public RabbitContext(MessageQueueSettings settings, ILogger logger) 
            {
                var rabbitConnFactory = new ConnectionFactory
                {
                    HostName = settings.Hostname,
                    UserName = settings.Username,
                    Password = settings.Password,
                    RequestedHeartbeat = 60,
                    AutomaticRecoveryEnabled = true,
                    TopologyRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
                };

                _connection = rabbitConnFactory.CreateConnection();
                _logger = logger;

                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Topic, durable: true, autoDelete: false, arguments: null);
                    channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false);
                    channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);
                }
            }

            public void Publish(string message) 
            {
                using (var channel = _connection.CreateModel())
                {
                    var bodyBytes = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.MessageId = Guid.NewGuid().ToString("N");
                    properties.Persistent = true;
                    properties.ContentType = "text/plain";

                    channel.BasicPublish(ExchangeName, RoutingKey, properties, bodyBytes);
                }
            }

            public IModel CreateChannel() => _connection.CreateModel();
            public void Dispose() => _connection.Dispose();

            // https://weblogs.asp.net/sweinstein/16-ways-to-create-iobservables-without-implementing-iobservable
            public Subscription<IMessage<string>> CreateSubscription() 
            {
                var compositeDisposable = new CompositeDisposable();   
                var subject = Subject.Synchronize(new ReplaySubject<IMessage<string>>());

                for(var i = 0; i < ConcurrencyLevel; i++)
                {
                    var channel = CreateChannel();
                    var consumer = new EventingBasicConsumer(channel);

                    // Per consumer limit. See http://www.rabbitmq.com/consumer-prefetch.html 
                    // and http://stackoverflow.com/a/8179850/463785
                    // Also see this for fetching more than one: http://stackoverflow.com/a/32592077/463785
                    channel.BasicQos(0, 1, false);

                    consumer.Received += (_, ea) => 
                    {
                        var message = new SimpleRabbitMessage(channel, ea);
                        _logger.LogDebug("Recieved {messageId}", message.Id);

                        // This is really a dispatch. Doesn't care for it to be consumed.
                        // So, we should really not Ack or Nack here.
                        // That's part of the consumer to handle.
                        subject.OnNext(message);
                        _logger.LogDebug("Dispatched {messageId} for handling", message.Id);
                    };

                    consumer.Shutdown += (_, ea) => 
                    {
                        _logger.LogWarning("Consumer has been shut down because of {cause} with {replyCode} for {replyText}", 
                            ea.Cause, 
                            ea.ReplyCode, 
                            ea.ReplyText);

                        subject.OnCompleted();
                    };

                    channel.BasicConsume(QueueName, noAck: false, consumer: consumer);

                    compositeDisposable.Add(channel);
                }

                return new Subscription<IMessage<string>>(subject, compositeDisposable);
            }
        }

        public class Subscription<TResult> : IDisposable
        {
            private readonly IDisposable _disposable;

            public Subscription(IObservable<TResult> observable, IDisposable disposable)
            {
                _disposable = disposable;
                Observable = observable;
            }

            public IObservable<TResult> Observable { get; }

            public void Dispose()
            {
                _disposable?.Dispose();
            }
        }
    }

    public class MessageQueueSettings
    {
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public static class ConfigBuilder
    {   
        public static IConfiguration Build()
        {
            return new ConfigurationBuilder()
                .SetBasePath(PlatformServices.Default.Application.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables("RxSample_")
                .Build();
        }
    }

    public static class LoggerFactoryBuilder
    {
        public static ILoggerFactory Build() 
        {
            var serilogLogger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Information()
                .CreateLogger();
            
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddSerilog(serilogLogger);

            return loggerFactory;
        }
    }
}
