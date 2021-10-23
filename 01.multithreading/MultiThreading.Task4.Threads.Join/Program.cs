/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static readonly int maxThreadsCount = 10;
        static int threadsCount = 1;
        static readonly Semaphore semaphore = new Semaphore(maxThreadsCount, maxThreadsCount);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            const int start = 50;

            Console.WriteLine("Task using threads");
            RunTaskWithThread(start);

            Console.WriteLine("Task using thread pool");
            RunTaskWithThreadPool(start);

            Console.ReadLine();
        }

        static void RunTaskWithThread(object parameters)
        {
            if (threadsCount > maxThreadsCount)
            {
                return;
            }

            Interlocked.Add(ref threadsCount, 1);
            var thread = new Thread(new ParameterizedThreadStart(RunTaskWithThread));
            int number = (int)parameters;
            ProcessNumber(ref number);
            thread.Start(number);
            thread.Join();
        }

        static void RunTaskWithThreadPool(object parameters)
        {
            if (semaphore.WaitOne())
            {
                int number = (int)parameters;
                ProcessNumber(ref number);
                ThreadPool.QueueUserWorkItem(new WaitCallback(RunTaskWithThreadPool), number);
            }
            
        }

        static void ProcessNumber(ref int number)
        {
            number--;
            Console.WriteLine("Number " + number);
        }
    }
}
