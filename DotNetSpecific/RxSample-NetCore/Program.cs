using System;
using System.Reactive.Subjects;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Comment out the sample you would like to run

            HelloWorld();
            // RunObserversSync().Wait();
            // RunObserversAsync().Wait();

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
    }
}
