using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions.Async
{
    public struct AsyncOption<T>
    {
        private Task<Option<T>> optionTask;

        internal AsyncOption(Task<Option<T>> optionTask)
        {
            this.optionTask = optionTask;
        }

        public TaskAwaiter<Option<T>> GetAwaiter()
        {
            return optionTask.GetAwaiter();
        }

        public Task<Option<T>> InnerTask
        {
            get { return optionTask; }
        }

        public AsyncOption<TResult> Map<TResult>(Func<T, TResult> mapping)
        {
            var newOptionTask = optionTask.Map(option => option.Map(mapping));
            return new AsyncOption<TResult>(newOptionTask);
        }

        public AsyncOption<TResult> FlatMap<TResult>(Func<T, Task<Option<TResult>>> mapping)
        {
            var newOptionTask = optionTask.FlatMap(option => option
                .Match(
                    some: value => mapping(value),
                    none: () => Task.FromResult(Option.None<TResult>())
                ));

            return new AsyncOption<TResult>(newOptionTask);
        }

        public AsyncOption<TResult> FlatMap<TResult>(Func<T, AsyncOption<TResult>> mapping)
        {
            return FlatMap(value => mapping(value).InnerTask);
        }

        public AsyncOption<TResult> FlatMap<TResult>(Func<T, Task<TResult>> mapping)
        {
            return FlatMap(value => mapping(value).Map(result => result.Some()));
        }

        public AsyncOption<TResult> FlatMap<TResult>(Func<T, Option<TResult>> mapping)
        {
            return FlatMap(value => Task.FromResult(mapping(value)));
        }
    }

    public static class AsyncOption
    {
        public static AsyncOption<T> FromTask<T>(Task<Option<T>> task)
        {
            return new AsyncOption<T>(task);
        }

        public static AsyncOption<T> Some<T>(T value)
        {
            return new AsyncOption<T>(Task.FromResult(Option.Some<T>(value)));
        }

        public static AsyncOption<T> Some<T>(Task<T> task)
        {
            return new AsyncOption<T>(task.Map(value => Option.Some(value)));
        }

        public static AsyncOption<T> None<T>()
        {
            return new AsyncOption<T>(Task.FromResult(Option.None<T>())); ;
        }
    }
}
