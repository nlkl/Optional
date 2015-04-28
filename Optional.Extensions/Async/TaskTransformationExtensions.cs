using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions.Async
{
    internal static class TaskTransformationExtensions
    {
        public static async Task<TResult> Map<TResult>(this Task task, Func<TResult> mapping)
        {
            await task;
            return mapping();
        }

        public static async Task Map(this Task task, Action mapping)
        {
            await task;
            mapping();
        }

        public static async Task<TResult> Map<T, TResult>(this Task<T> task, Func<T, TResult> mapping)
        {
            return mapping(await task);
        }

        public static async Task Map<T>(this Task<T> task, Action<T> mapping)
        {
            mapping(await task);
        }

        public static async Task FlatMap(this Task task, Func<Task> mapping)
        {
            await task;
            await mapping();
        }
        
        public static async Task<TResult> FlatMap<TResult>(this Task task, Func<Task<TResult>> mapping)
        {
            await task;
            return await mapping();
        }

        public static async Task FlatMap<T>(this Task<T> task, Func<T, Task> mapping)
        {
            await mapping(await task);
        }
        
        public static async Task<TResult> FlatMap<T, TResult>(this Task<T> task, Func<T, Task<TResult>> mapping)
        {
            return await mapping(await task);
        }
    }
}
