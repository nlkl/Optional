using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions.Async
{
    public static class TaskExtensions
    {
        public static Task<TResult> Map<TResult>(this Task task, Func<TResult> mapping)
        {
            return Task.FromResult(mapping());
        }

        public static async Task<TResult> Map<T, TResult>(this Task<T> task, Func<T, TResult> mapping)
        {
            return mapping(await task);
        }

        public static Task FlatMap(this Task task, Func<Task> mapping)
        {
            return mapping();
        }
        
        public static Task<TResult> FlatMap<TResult>(this Task task, Func<Task<TResult>> mapping)
        {
            return mapping();
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
