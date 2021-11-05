using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens
{
    static class Calculator
    {
        // todo: change this method to support cancellation token
        public static long Calculate(int n, CancellationToken token)
        {
            long sum = 0;

            var options = new ParallelOptions() { CancellationToken = token };
            Parallel.For(0, n, options, (i) =>
            {
                options.CancellationToken.ThrowIfCancellationRequested();
                Interlocked.Add(ref sum, i + 1);
                Thread.Sleep(100);
            });

            return sum;
        }
    }
}
