using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyTask
{
    class Program
    {
        private static readonly int _tasksNumber = 30;

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            RunProcessTask();
            stopwatch.Stop();
            Console.WriteLine($"Task execution time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            RunProcessThreadPool();
            stopwatch.Stop();
            Console.WriteLine($"ThreadPool execution time {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            RunProcessParallel();
            stopwatch.Stop();
            Console.WriteLine($"Parallel execution time: {stopwatch.ElapsedMilliseconds} ms");

            Console.ReadKey();
        }

        private static void RunProcessTask()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < _tasksNumber; i++)
            {
                Task task = ProcessAsync();
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        private static void RunProcessThreadPool()
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

        private static void RunProcessParallel()
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

        private static async Task ProcessAsync()
        {
            await Task.Run(() => Thread.Sleep(1000));
        }
    }
}
