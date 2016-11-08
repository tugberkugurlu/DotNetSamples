using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace RxSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Comment out the sample you would like to run

            // HelloWorld();
            // RunObserversSync().Wait();
            RunObserversAsync().Wait();

            Console.ReadLine();
        }

        private static void HelloWorld()
        {
            var subject = new Subject<int>();
            subject.OnNext(42);
            var s1 = subject.Subscribe(x => Console.WriteLine($"1> {x}"));
            subject.OnNext(43);
            var s2 = subject.Subscribe(x => Console.WriteLine($"2> {x}"));
            subject.OnNext(44);
            s1.Dispose();
            subject.OnNext(45);
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
    }
}
