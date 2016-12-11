using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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
            RabbitMqObserver(logger).Wait();
            Console.ReadLine();
        }

        // http://stackoverflow.com/questions/16658915/reactive-extensions-concurrency-within-the-subscriber
        private static async Task RabbitMqObserver(ILogger logger, CancellationToken cancellationToken = default(CancellationToken))
        {
            logger.LogInformation("ConcurrencyLevel: {concurrencyLevel}", ConcurrencyLevel);
            Sleep(logger, TimeSpan.FromSeconds(10), "to let RabbitMQ start up");

            var config = ConfigBuilder.Build();
            var settings = new MessageQueueSettings();
            ConfigurationBinder.Bind(config.GetSection("MessageQueue"), settings);

            using(var context = new RabbitContext(settings))
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
                    using(var subscription = context.CreateSubscription())
                    using(subscription.Observable.ObserveOn(TaskPoolScheduler.Default).Subscribe(message => { Thread.Sleep(3000); Console.WriteLine(message); }))
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

            public RabbitContext(MessageQueueSettings settings) 
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
                    properties.Persistent = true;
                    properties.ContentType = "text/plain";

                    channel.BasicPublish(ExchangeName, RoutingKey, properties, bodyBytes);
                }
            }

            public IModel CreateChannel() => _connection.CreateModel();
            public void Dispose() => _connection.Dispose();

            public Subscription<string> CreateSubscription() 
            {
                var compositeDisposable = new CompositeDisposable();   
                var subject = Subject.Synchronize(new ReplaySubject<string>());

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
                        var message = Encoding.UTF8.GetString(ea.Body);
                        subject.OnNext(message);
                        channel.BasicAck(ea.DeliveryTag, multiple: false);
                    };

                    channel.BasicConsume(QueueName, noAck: false, consumer: consumer);

                    compositeDisposable.Add(channel);
                }

                return new Subscription<string>(subject, compositeDisposable);
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
                .MinimumLevel.Verbose()
                .CreateLogger();
            
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddSerilog(serilogLogger);

            return loggerFactory;
        }
    }
}
