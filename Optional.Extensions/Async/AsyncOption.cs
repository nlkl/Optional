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

    public struct AsyncOption<T, TException>
    {
        private Task<Option<T, TException>> optionTask;

        internal AsyncOption(Task<Option<T, TException>> optionTask)
        {
            this.optionTask = optionTask;
        }

        public TaskAwaiter<Option<T, TException>> GetAwaiter()
        {
            return optionTask.GetAwaiter();
        }

        public Task<Option<T, TException>> InnerTask
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

        public AsyncOption<T, TException> Or(T alternative)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Or(alternative)));
        }

        public AsyncOption<T, TException> Or(Func<T> alternativeFactory)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Or(alternativeFactory)));
        }

        public Task<TResult> Match<TResult>(Func<T, TResult> some, Func<TException, TResult> none)
        {
            return optionTask.Map(option => option.Match(some, none));
        }

        public Task Match(Action<T> some, Action<TException> none)
        {
            return optionTask.Map(option => option.Match(some, none));
        }

        public AsyncOption<TResult, TException> Map<TResult>(Func<T, TResult> mapping)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Map(mapping)));
        }

        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Task<Option<TResult, TException>>> mapping)
        {
            return AsyncOption.FromTask(optionTask.FlatMap(option => option
                .Match(
                    some: value => mapping(value),
                    none: exception => Task.FromResult(Option.None<TResult, TException>(exception))
                )));
        }

        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Task<Option<TResult>>> mapping, TException exception)
        {
            return FlatMap(value => mapping(value).Map(option => option.WithException(exception)));
        }

        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, AsyncOption<TResult, TException>> mapping)
        {
            return FlatMap(value => mapping(value).InnerTask);
        }

        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, AsyncOption<TResult>> mapping, TException exception)
        {
            return FlatMap(value => mapping(value).InnerTask.Map(option => option.WithException(exception)));
        }

        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Task<TResult>> mapping)
        {
            return FlatMap(value => mapping(value).Map(result => Option.Some<TResult, TException>(result)));
        }

        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Option<TResult, TException>> mapping)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.FlatMap(mapping)));
        }

        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Option<TResult>> mapping, TException exception)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.FlatMap(mapping, exception)));
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

        public static AsyncOption<T, TException> FromTask<T, TException>(Task<Option<T, TException>> task)
        {
            return new AsyncOption<T, TException>(task);
        }

        public static AsyncOption<T, TException> FromTask<T, TException>(Func<Task<Option<T, TException>>> taskFactory)
        {
            return new AsyncOption<T, TException>(taskFactory());
        }

        public static AsyncOption<T, TException> Some<T, TException>(T value)
        {
            return new AsyncOption<T, TException>(Task.FromResult(Option.Some<T, TException>(value)));
        }

        public static AsyncOption<T, TException> Some<T, TException>(Task<T> task)
        {
            return new AsyncOption<T, TException>(task.Map(value => Option.Some<T, TException>(value)));
        }

        public static AsyncOption<T, TException> None<T, TException>(TException exception)
        {
            return new AsyncOption<T, TException>(Task.FromResult(Option.None<T, TException>(exception)));
        }
    }
}
