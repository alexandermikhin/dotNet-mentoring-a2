/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    enum BreakType
    {
        Break = 0,
        ThrowException = 1,
        Cancel = 2,
    }

    class Program
    {
        static readonly int delay = 500;
        static BreakType breakType;

        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            RunTasks();

            Console.ReadLine();
        }

        static void RunTasks()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancelTask = Task.Factory.StartNew(() => CancelTask(cancellationTokenSource));

            var task = Task.Factory.StartNew(() => MainTaskThread(cancellationTokenSource.Token), cancellationTokenSource.Token);
            var taskA = task.ContinueWith(t => TaskAThread(), TaskContinuationOptions.None);
            var taskB = task.ContinueWith(t => TaskBThread(), TaskContinuationOptions.OnlyOnFaulted);
            var taskC = task.ContinueWith(t => TaskCThread(), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            var taskD = task.ContinueWith(t => TaskDThread(), TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

            taskA.Wait();
        }

        static void MainTaskThread(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    switch (breakType)
                    {
                        case BreakType.Break:
                            Console.WriteLine("Breaking.");
                            return;
                        case BreakType.Cancel:
                            Console.WriteLine("Cancelling.");
                            token.ThrowIfCancellationRequested();
                            break;
                        case BreakType.ThrowException:
                            Console.WriteLine("Throwing exception.");
                            throw null;
                    }
                }

                Thread.Sleep(delay);
                Console.WriteLine("Main task running. Thread id " + Thread.CurrentThread.ManagedThreadId);
            }
        }

        static void TaskAThread()
        {
            Console.WriteLine("Task A. Thread id " + Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(2000);

        }

        static void TaskBThread()
        {
            Console.WriteLine("Task B. Thread id " + Thread.CurrentThread.ManagedThreadId);
        }

        static void TaskCThread()
        {
            Console.WriteLine("Task C. Thread id " + Thread.CurrentThread.ManagedThreadId);
        }

        static void TaskDThread()
        {
            Console.WriteLine("Task D. Thread id " + Thread.CurrentThread.ManagedThreadId);
        }

        static void CancelTask(CancellationTokenSource source)
        {
            Console.WriteLine("Press 'x' to cancel, 't' to throw exception, any other key to break...");
            var keyInfo = Console.ReadKey();
            switch (keyInfo.KeyChar)
            {
                case 'x': breakType = BreakType.Cancel;
                    break;
                case 't': breakType = BreakType.ThrowException;
                    break;
                default: breakType = BreakType.Break;
                    break;
            }

            source.Cancel();
        }
    }
}
