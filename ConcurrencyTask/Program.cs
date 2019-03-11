using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyTask
{
    class Program
    {
        private static readonly int _tasksNumber = 3;

        static async Task Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await ProcessAsync();
            stopwatch.Stop();
            Console.WriteLine($"Async execution time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            ProcessThreadPool();
            stopwatch.Stop();
            Console.WriteLine($"ThreadPool execution time {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            ProcessParallel();
            stopwatch.Stop();
            Console.WriteLine($"Parallel execution time: {stopwatch.ElapsedMilliseconds} ms");

            Console.ReadKey();
        }

        private static async Task ProcessAsync()
        {
            for (int i = 0; i < _tasksNumber; i++)
            {
                await Task.Run(() =>
                {
                    Process();
                });
            }
        }

        private static void ProcessThreadPool()
        {
            using (CountdownEvent signaler = new CountdownEvent(_tasksNumber))
            {
                for (int i = 0; i < _tasksNumber; i++)
                {
                    ThreadPool.QueueUserWorkItem(x =>
                    {
                        Process();
                        signaler.Signal();
                    });
                }

                signaler.Wait();
            }
        }

        private static void ProcessParallel()
        {
            Parallel.For(0, _tasksNumber, i =>
            {
                Process();
            });
        }

        private static void Process()
        {
            Thread.Sleep(1000);
        }
    }
}
