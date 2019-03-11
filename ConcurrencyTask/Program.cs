using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyTask
{
    class Program
    {
        private static readonly int _tasksNumber = 3;
        private static Stopwatch _threadPoolStopwatch;

        static async Task Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await ProcessAsync();
            stopwatch.Stop();
            Console.WriteLine($"Async execution time: {stopwatch.ElapsedMilliseconds} ms");

            _threadPoolStopwatch = new Stopwatch();
            _threadPoolStopwatch.Start();
            ProcessThreadPool();

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
                    Process("Async_" + i);
                });
            }
        }

        private static void ProcessThreadPool()
        {
            for (int i = 0; i < _tasksNumber; i++)
            {
                ThreadPool.QueueUserWorkItem(Process, "ThreadPool_" + i);
            }
        }

        private static void ProcessParallel()
        {
            Parallel.For(0, _tasksNumber, i =>
            {
                Process("Parallel_" + i);
            });
        }

        private static void Process(object processType)
        {
            Thread.Sleep(1000);
            if ((string)processType == "ThreadPool_2")
            {
                _threadPoolStopwatch.Stop();
                Console.WriteLine($"ThreadPool execution time: {_threadPoolStopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}
