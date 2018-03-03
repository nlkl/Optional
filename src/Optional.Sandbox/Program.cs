using System;
using System.Threading;
using System.Threading.Tasks;
using Optional;
using Optional.Async.Linq;

namespace Optional.Sandbox
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var ctx = SynchronizationContext.Current;

            var value = await
                from value1 in Task.FromResult(22.Some())
                from value2 in Task.FromResult((value1 + 3).Some())
                select value2;

            Console.WriteLine(value);

            Console.Read();
        }
    }
}
