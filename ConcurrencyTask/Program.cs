using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyTask
{
    class Program
    {
        private static DateTime _startTime;
        private static readonly int _tasksNumber = 12;

        static async Task Main(string[] args)
        {
            _startTime = DateTime.Now;
            await ProcessAsync();

            _startTime = DateTime.Now;
            ProcessThreadPool();

            _startTime = DateTime.Now;
            ProcessParallel();

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
            Console.WriteLine("ThreadPool finished");
        }

        private static void ProcessParallel()
        {
            Parallel.For(0, _tasksNumber, i =>
            {
                Process("Parallel_" + i);
            });
            Console.WriteLine("Parallel finished");
        }

        private static void Process(object processType)
        {
            Console.WriteLine($"{processType} started");
            Thread.Sleep(2000);
            var executionTime = DateTime.Now - _startTime;
            Console.WriteLine($"{processType} execution time: {executionTime.TotalMilliseconds} ms");
        }
    }
}
