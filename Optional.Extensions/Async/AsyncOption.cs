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

        public Task<bool> Contains(T value)
        {
            return optionTask.Map(option => option.Contains(value));
        }

        public Task<bool> Exists(Func<T, bool> predicate)
        {
            return optionTask.Map(option => option.Exists(predicate));
        }

        public Task<T> ValueOr(T alternative)
        {
            return optionTask.Map(option => option.ValueOr(alternative)); 
        }

        public Task<T> ValueOr(Func<T> alternativeFactory)
        {
            return optionTask.Map(option => option.ValueOr(alternativeFactory));
        }

        public AsyncOption<T> Or(T alternative)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Or(alternative)));
        }

        public AsyncOption<T> Or(Func<T> alternativeFactory)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Or(alternativeFactory)));
        }

        public Task<TResult> Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            return optionTask.Map(option => option.Match(some, none));
        }

        public Task Match(Action<T> some, Action none)
        {
            return optionTask.Map(option => option.Match(some, none));
        }

        public AsyncOption<TResult> Map<TResult>(Func<T, TResult> mapping)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Map(mapping)));
        }

        public AsyncOption<TResult> FlatMap<TResult>(Func<T, Task<Option<TResult>>> mapping)
        {
            return AsyncOption.FromTask(optionTask.FlatMap(option => option
                .Match(
                    some: value => mapping(value),
                    none: () => Task.FromResult(Option.None<TResult>())
                )));
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
            return AsyncOption.FromTask(optionTask.Map(option => option.FlatMap(mapping)));
        }
    }

    public static class AsyncOption
    {
        public static AsyncOption<T> FromTask<T>(Task<Option<T>> task)
        {
            return new AsyncOption<T>(task);
        }

        public static AsyncOption<T> FromTask<T>(Func<Task<Option<T>>> taskFactory)
        {
            return new AsyncOption<T>(taskFactory());
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
