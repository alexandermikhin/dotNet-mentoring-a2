/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static readonly List<byte> items = new List<byte>();
        static readonly AutoResetEvent generateEvent = new AutoResetEvent(false);
        static readonly AutoResetEvent printEvent = new AutoResetEvent(true);
        static readonly ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();
        static readonly byte count = 10;
        static bool finished = false;

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var generateTask = Task.Factory.StartNew(GenerateNumbers);
            var printTask = Task.Factory.StartNew(PrintNumbers);
            Task.WaitAll(generateTask, printTask);

            Console.ReadLine();
        }

        static void GenerateNumbers()
        {
            for (byte i = 1; i <= count; i++)
            {
                printEvent.WaitOne();
                readerWriterLockSlim.EnterWriteLock();
                items.Add(i);
                readerWriterLockSlim.ExitWriteLock();
                generateEvent.Set();
            }

            finished = true;
        }

        static void PrintNumbers()
        {
            while (!finished)
            {
                generateEvent.WaitOne();
                readerWriterLockSlim.EnterReadLock();
                var printString = GetPrintString(items);
                Console.WriteLine(printString);
                readerWriterLockSlim.ExitReadLock();
                printEvent.Set();
            }
        }

        static string GetPrintString(List<byte> values)
        {
            return "[ " + string.Join(", ", values) + " ]";
        }
    }
}
