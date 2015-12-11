using System;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;

namespace RabbitMqEasyNetQSample
{
    class Program
    {
        // EasyNetQ Quick Start: https://github.com/EasyNetQ/EasyNetQ/wiki/Quick-Start
        // Management Console: http://localhost:15672/

        // Exchange: where the message are being published
        // Queue: where the exchgaes are being directed to

        // What I need:
        //
        // (YES) A message broker where I publish messages
        // (YES) A message broker where I subscribe to messages
        // When a consumer is processing a message, it can get a failure and process can shut down. 
        //      In those cases, message shouldn't be lost.
        // When a consumer is processing a message, 
        //      it can get a temporary failure. In those cases, the message
        //      should be retriable.

        static void Main(string[] args)
        {
            var pubTask = new Publisher().Start();
            var subTask = new Subscriber().Start();
            Task.WhenAll(pubTask, subTask).Wait();
        }
    }

    public class TextMessage
    {
        public string Text { get; set; }
    }

    public class Publisher
    {
        public Task Start()
        {
            return Task.Run(() =>
            {
                using (var bus = RabbitHutch.CreateBus("host=localhost"))
                {
                    while (true)
                    {
                        var input = Faker.Lorem.Sentence();
                        bus.Publish(new TextMessage
                        {
                            Text = input
                        });

                        Thread.Sleep(1000);
                    }
                }
            });
        }
    }

    public class Subscriber
    {
        public Task Start()
        {
            return Task.Run(() =>
            {
                using (var bus = RabbitHutch.CreateBus("host=localhost"))
                {
                    bus.Subscribe<TextMessage>("test", HandleTextMessage);
                    Thread.Sleep(-1);
                }
            });
        }

        private static void HandleTextMessage(TextMessage textMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Subscriber] Got message: {0}", textMessage.Text);
            Console.ResetColor();
        }
    }

    public interface IExchange
    {

    }
}
