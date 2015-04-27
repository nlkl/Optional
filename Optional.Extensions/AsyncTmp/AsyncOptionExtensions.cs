using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Optional.Extensions.Async;

namespace Optional.Extensions.AsyncTmp
{
    public static class AsyncOptionExtensions
    {
        public static AsyncOption<T> Collapse<T>(this Task<Option<T>> task)
        {
            return AsyncOption.FromTask(task);
        }
    }
}
