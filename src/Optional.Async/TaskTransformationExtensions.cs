using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Async
{
    internal static class TaskTransformationExtensions
    {
        public static async Task<TResult> Map<TResult>(this Task task, Func<TResult> mapping)
        {
            await task.ConfigureAwait(false);
            return mapping();
        }

        public static async Task Map(this Task task, Action mapping)
        {
            await task.ConfigureAwait(false);
            mapping();
        }

        public static async Task<TResult> Map<T, TResult>(this Task<T> task, Func<T, TResult> mapping)
        {
            var value = await task.ConfigureAwait(false);
            return mapping(value);
        }

        public static async Task Map<T>(this Task<T> task, Action<T> mapping)
        {
            var value = await task.ConfigureAwait(false);
            mapping(value);
        }

        public static async Task FlatMap(this Task task, Func<Task> mapping)
        {
            await task.ConfigureAwait(false);
            await mapping().ConfigureAwait(false);
        }
        
        public static async Task<TResult> FlatMap<TResult>(this Task task, Func<Task<TResult>> mapping)
        {
            await task.ConfigureAwait(false);
            return await mapping().ConfigureAwait(false);
        }

        public static async Task FlatMap<T>(this Task<T> task, Func<T, Task> mapping)
        {
            var value = await task.ConfigureAwait(false);
            await mapping(value).ConfigureAwait(false);
        }
        
        public static async Task<TResult> FlatMap<T, TResult>(this Task<T> task, Func<T, Task<TResult>> mapping)
        {
            var value = await task.ConfigureAwait(false);
            return await mapping(value).ConfigureAwait(false);
        }
    }
}
