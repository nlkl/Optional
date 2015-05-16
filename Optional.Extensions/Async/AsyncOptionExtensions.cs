using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions.Async
{
    public static class AsyncOptionExtensions
    {
        /// <summary>
        /// Creates an async optional.
        /// </summary>
        /// <param name="task">The task to construct the async optional from.</param>
        /// <returns>The async optional.</returns>
        public static AsyncOption<T> ToAsyncOption<T>(this Task<Option<T>> task)
        {
            return AsyncOption.FromTask(task);
        }

        /// <summary>
        /// Creates an async optional.
        /// </summary>
        /// <param name="task">The task to construct the async optional from.</param>
        /// <returns>The async optional.</returns
        public static AsyncOption<T, TException> ToAsyncOption<T, TException>(this Task<Option<T, TException>> task)
        {
            return AsyncOption.FromTask(task);
        }
    }
}
