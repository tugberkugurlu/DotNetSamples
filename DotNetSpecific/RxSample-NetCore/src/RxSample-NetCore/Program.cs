using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsoleApplication
{
    public class Program
    {
        private static readonly int ConcurrencyLevel = Environment.ProcessorCount * 2;

        public static void Main(string[] args)
        {
            // Comment out the sample you would like to run

            // HelloWorld();
            // RunObserversSync();
            // RunObserversAsync().Wait();

            RabbitMqObserver().Wait();

            Console.ReadLine();
        }

        private static void HelloWorld()
        {
            using(var subject = new Subject<int>())
            {
                // this will be fired to nowhere as there are no consumers
                subject.OnNext(42);

                var s1 = subject.Subscribe(x => Console.WriteLine($"s1> {x}"));

                // s1 will consume this
                subject.OnNext(43);

                var s2 = subject.Subscribe(x => Console.WriteLine($"s2> {x}"));

                // s1 and s2 will consume this
                subject.OnNext(44);
                s1.Dispose();

                // s2 will consume this
                subject.OnNext(45);

                s2.Dispose();
            }
        }

        private static void RunObserversSync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // With this way, each observer runs syncronously one at a time.
            // This is not really great for concurrent cases.

            using (var compositeDisposable = new CompositeDisposable())
            {
                var subject = new Subject<int>();
                for (int i = 0; i < 100; i++)
                {
                    var subName = i;
                    var subscription = subject.Subscribe(x =>
                    {
                        Thread.Sleep(5000);
                        Console.WriteLine($"{subName}> {x}");
                    });

                    compositeDisposable.Add(subscription);
                }

                var random = new Random();
                do
                {
                    subject.OnNext(random.Next());
                } while (cancellationToken.IsCancellationRequested == false);
            }
        }


        // http://stackoverflow.com/questions/16658915/reactive-extensions-concurrency-within-the-subscriber
        private static async Task RunObserversAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(4000))
                .Do(x => Console.WriteLine("{0} Thread: {1} Source value: {2}",
                    DateTime.Now,
                    Thread.CurrentThread.ManagedThreadId, x))
                .ObserveOn(NewThreadScheduler.Default);

            using (var compositeDisposable = new CompositeDisposable())
            {
                for (int i = 0; i < 100; i++)
                {
                    var subName = i;
                    var subscription = source.Subscribe(x =>
                    {
                        Thread.Sleep(5000);
                        Console.WriteLine($"{subName}> {x}");
                    });

                    compositeDisposable.Add(subscription);
                }

                try
                {
                    await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                }
                catch (TaskCanceledException) { }
            }
        }

        private static async Task RabbitMqObserver(CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine($"ConcurrencyLevel: {ConcurrencyLevel}");
            Sleep(TimeSpan.FromSeconds(5), "to let RabbitMQ start up");

            var config = ConfigBuilder.Build();
            var settings = new MessageQueueSettings();
            ConfigurationBinder.Bind(config.GetSection("MessageQueue"), settings);

            using(var context = new RabbitContext(settings))
            {
                var publisher = Task.Run(async () => 
                {
                    do
                    {
                        context.Publish(new Bogus.Faker().Lorem.Sentence(100));
                        await Task.Delay(TimeSpan.FromMilliseconds(100));

                    } while (cancellationToken.IsCancellationRequested == false);   
                });

                var subscriber = Task.Run(async () => 
                {
                    using(var channel = context.CreateChannel())
                    {
                        var observable = RabbitContext.CreateSubject(channel);
                        using(observable.Subscribe(message => { Thread.Sleep(3000); Console.WriteLine(message); }))
                        {
                            await SleepTillCancelledAsync(cancellationToken);
                        }
                    }
                });

                await Task.WhenAll(publisher, subscriber);
            }
        }

        private static void Sleep(TimeSpan sleepAmount, string message) 
        {
            Console.WriteLine($"Waiting for {sleepAmount}: {message}");
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

            public static IObservable<string> CreateSubject(IModel channel) 
            {
                if(channel == null) throw new ArgumentNullException(nameof(channel));
                
                var subject = new ReplaySubject<string>();
                var consumer = new EventingBasicConsumer(channel);

                // Per consumer limit. See http://www.rabbitmq.com/consumer-prefetch.html 
                // and http://stackoverflow.com/a/8179850/463785
                // Also see this for multithreading: http://stackoverflow.com/a/32592077/463785
                channel.BasicQos(0, (ushort)(ConcurrencyLevel), false);

                consumer.Received += (_, ea) => 
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    subject.OnNext(message);
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(QueueName, noAck: false, consumer: consumer);

                return subject;
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
}
