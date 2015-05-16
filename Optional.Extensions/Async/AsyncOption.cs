using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions.Async
{
    /// <summary>
    /// Represents an asynchronous optional value.
    /// </summary>
    /// <typeparam name="T">The type of the value to be wrapped.</typeparam>
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

        public Task<Option<T>> InnerTask { get { return optionTask; } }

        /// <summary>
        /// Checks if a value is present.
        /// </summary>
        public Task<bool> HasValue { get { return optionTask.Map(option => option.HasValue); } }

        /// <summary>
        /// Determines if the current optional contains a specified value.
        /// </summary>
        /// <param name="value">The value to locate.</param>
        /// <returns>A boolean indicating whether or not the value was found.</returns>
        public Task<bool> Contains(T value)
        {
            return optionTask.Map(option => option.Contains(value));
        }

        /// <summary>
        /// Determines if the current optional contains a value 
        /// satisfying a specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A boolean indicating whether or not the predicate was satisfied.</returns>
        public Task<bool> Exists(Func<T, bool> predicate)
        {
            return optionTask.Map(option => option.Exists(predicate));
        }

        /// <summary>
        /// Returns the existing value if present, and otherwise an alternative value.
        /// </summary>
        /// <param name="alternative">The alternative value.</param>
        /// <returns>The existing or alternative value.</returns>
        public Task<T> ValueOr(T alternative)
        {
            return optionTask.Map(option => option.ValueOr(alternative));
        }

        /// <summary>
        /// Returns the existing value if present, and otherwise an alternative value.
        /// </summary>
        /// <param name="alternativeFactory">A factory function to create an alternative value.</param>
        /// <returns>The existing or alternative value.</returns>
        public Task<T> ValueOr(Func<T> alternativeFactory)
        {
            return optionTask.Map(option => option.ValueOr(alternativeFactory));
        }

        /// <summary>
        /// Uses an alternative value, if no existing value is present.
        /// </summary>
        /// <param name="alternative">The alternative value.</param>
        /// <returns>A new optional, containing either the existing or alternative value.</returns>
        public AsyncOption<T> Or(T alternative)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Or(alternative)));
        }

        /// <summary>
        /// Uses an alternative value, if no existing value is present.
        /// </summary>
        /// <param name="alternativeFactory">A factory function to create an alternative value.</param>
        /// <returns>A new optional, containing either the existing or alternative value.</returns>
        public AsyncOption<T> Or(Func<T> alternativeFactory)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Or(alternativeFactory)));
        }

        /// <summary>
        /// Attaches an exceptional value to an empty optional.
        /// </summary>
        /// <param name="exception">The exceptional value to attach.</param>
        /// <returns>An optional with an exceptional value.</returns>
        public AsyncOption<T, TException> WithException<TException>(TException exception)
        {
            return AsyncOption.FromTask(InnerTask.Map(option => option.WithException(exception)));
        }

        /// <summary>
        /// Attaches an exceptional value to an empty optional.
        /// </summary>
        /// <param name="exceptionFactory">A factory function to create an exceptional value to attach.</param>
        /// <returns>An optional with an exceptional value.</returns>
        public AsyncOption<T, TException> WithException<TException>(Func<TException> exceptionFactory)
        {
            return AsyncOption.FromTask(InnerTask.Map(option => option.WithException(exceptionFactory)));
        }

        /// <summary>
        /// Evaluates a specified function, based on whether a value is present or not.
        /// </summary>
        /// <param name="some">The function to evaluate if the value is present.</param>
        /// <param name="none">The function to evaluate if the value is missing.</param>
        /// <returns>The result of the evaluated function.</returns>
        public Task<TResult> Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            return optionTask.Map(option => option.Match(some, none));
        }

        /// <summary>
        /// Evaluates a specified action, based on whether a value is present or not.
        /// </summary>
        /// <param name="some">The action to evaluate if the value is present.</param>
        /// <param name="none">The action to evaluate if the value is missing.</param>
        public Task Match(Action<T> some, Action none)
        {
            return optionTask.Map(option => option.Match(some, none));
        }

        /// <summary>
        /// Transforms the inner value in an optional.
        /// If the instance is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult> Map<TResult>(Func<T, TResult> mapping)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Map(mapping)));
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult> FlatMap<TResult>(Func<T, Task<Option<TResult>>> mapping)
        {
            return AsyncOption.FromTask(optionTask.FlatMap(option => option
                .Match(
                    some: value => mapping(value),
                    none: () => Task.FromResult(Option.None<TResult>())
                )));
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// If the option contains an exception, it is removed.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult> FlatMap<TResult, TException>(Func<T, Task<Option<TResult, TException>>> mapping)
        {
            return FlatMap(value => mapping(value).Map(option => option.WithoutException()));
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult> FlatMap<TResult>(Func<T, AsyncOption<TResult>> mapping)
        {
            return FlatMap(value => mapping(value).InnerTask);
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// If the option contains an exception, it is removed.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult> FlatMap<TResult, TException>(Func<T, AsyncOption<TResult, TException>> mapping)
        {
            return FlatMap(value => mapping(value).InnerTask);
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult> FlatMap<TResult>(Func<T, Option<TResult>> mapping)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.FlatMap(mapping)));
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// If the option contains an exception, it is removed.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult> FlatMap<TResult, TException>(Func<T, Option<TResult, TException>> mapping)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.FlatMap(mapping)));
        }

        public AsyncOption<TResult> FlatMap<TResult>(Func<T, Task<TResult>> mapping)
        {
            return FlatMap(value => mapping(value).Map(result => result.Some()));
        }

        /// <summary>
        /// Empties an optional, if a specified predicate
        /// is not satisfied.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The filtered optional.</returns>
        public AsyncOption<T> Filter(Func<T, bool> predicate)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Filter(predicate)));
        }
    }

    /// <summary>
    /// Represents an asynchronous optional value, along with a potential exceptional value.
    /// </summary>
    /// <typeparam name="T">The type of the value to be wrapped.</typeparam>
    /// <typeparam name="TException">A exceptional value describing the lack of an actual value.</typeparam>
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

        public Task<Option<T, TException>> InnerTask { get { return optionTask; } }

        /// <summary>
        /// Checks if a value is present.
        /// </summary>
        public Task<bool> HasValue { get { return optionTask.Map(option => option.HasValue); } }

        /// <summary>
        /// Determines if the current optional contains a specified value.
        /// </summary>
        /// <param name="value">The value to locate.</param>
        /// <returns>A boolean indicating whether or not the value was found.</returns>
        public Task<bool> Contains(T value)
        {
            return optionTask.Map(option => option.Contains(value));
        }

        /// <summary>
        /// Determines if the current optional contains a value 
        /// satisfying a specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A boolean indicating whether or not the predicate was satisfied.</returns>
        public Task<bool> Exists(Func<T, bool> predicate)
        {
            return optionTask.Map(option => option.Exists(predicate));
        }

        /// <summary>
        /// Returns the existing value if present, and otherwise an alternative value.
        /// </summary>
        /// <param name="alternative">The alternative value.</param>
        /// <returns>The existing or alternative value.</returns>
        public Task<T> ValueOr(T alternative)
        {
            return optionTask.Map(option => option.ValueOr(alternative));
        }

        /// <summary>
        /// Returns the existing value if present, and otherwise an alternative value.
        /// </summary>
        /// <param name="alternativeFactory">A factory function to create an alternative value.</param>
        /// <returns>The existing or alternative value.</returns>
        public Task<T> ValueOr(Func<T> alternativeFactory)
        {
            return optionTask.Map(option => option.ValueOr(alternativeFactory));
        }

        /// <summary>
        /// Uses an alternative value, if no existing value is present.
        /// </summary>
        /// <param name="alternative">The alternative value.</param>
        /// <returns>A new optional, containing either the existing or alternative value.</returns>
        public AsyncOption<T, TException> Or(T alternative)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Or(alternative)));
        }

        /// <summary>
        /// Uses an alternative value, if no existing value is present.
        /// </summary>
        /// <param name="alternativeFactory">A factory function to create an alternative value.</param>
        /// <returns>A new optional, containing either the existing or alternative value.</returns>
        public AsyncOption<T, TException> Or(Func<T> alternativeFactory)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Or(alternativeFactory)));
        }

        /// <summary>
        /// Forgets any attached exceptional value.
        /// </summary>
        /// <returns>An optional without an exceptional value.</returns>
        public AsyncOption<T> WithoutException()
        {
            return AsyncOption.FromTask(InnerTask.Map(option => option.WithoutException()));
        }

        /// <summary>
        /// Evaluates a specified function, based on whether a value is present or not.
        /// </summary>
        /// <param name="some">The function to evaluate if the value is present.</param>
        /// <param name="none">The function to evaluate if the value is missing.</param>
        /// <returns>The result of the evaluated function.</returns>
        public Task<TResult> Match<TResult>(Func<T, TResult> some, Func<TException, TResult> none)
        {
            return optionTask.Map(option => option.Match(some, none));
        }

        /// <summary>
        /// Evaluates a specified action, based on whether a value is present or not.
        /// </summary>
        /// <param name="some">The action to evaluate if the value is present.</param>
        /// <param name="none">The action to evaluate if the value is missing.</param>
        public Task Match(Action<T> some, Action<TException> none)
        {
            return optionTask.Map(option => option.Match(some, none));
        }

        /// <summary>
        /// Transforms the inner value in an optional.
        /// If the instance is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult, TException> Map<TResult>(Func<T, TResult> mapping)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Map(mapping)));
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Task<Option<TResult, TException>>> mapping)
        {
            return AsyncOption.FromTask(optionTask.FlatMap(option => option
                .Match(
                    some: value => mapping(value),
                    none: exception => Task.FromResult(Option.None<TResult, TException>(exception))
                )));
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <param name="exception">The exceptional value to attach.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Task<Option<TResult>>> mapping, TException exception)
        {
            return FlatMap(value => mapping(value).Map(option => option.WithException(exception)));
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <param name="exceptionFactory">A factory function to create an exceptional value to attach.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Task<Option<TResult>>> mapping, Func<TException> exceptionFactory)
        {
            return FlatMap(value => mapping(value).Map(option => option.WithException(exceptionFactory)));
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, AsyncOption<TResult, TException>> mapping)
        {
            return FlatMap(value => mapping(value).InnerTask);
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <param name="exception">The exceptional value to attach.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, AsyncOption<TResult>> mapping, TException exception)
        {
            return FlatMap(value => mapping(value).InnerTask, exception);
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <param name="exceptionFactory">A factory function to create an exceptional value to attach.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, AsyncOption<TResult>> mapping, Func<TException> exceptionFactory)
        {
            return FlatMap(value => mapping(value).InnerTask, exceptionFactory);
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Option<TResult, TException>> mapping)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.FlatMap(mapping)));
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <param name="exception">The exceptional value to attach.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Option<TResult>> mapping, TException exception)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.FlatMap(mapping, exception)));
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <param name="exceptionFactory">A factory function to create an exceptional value to attach.</param>
        /// <returns>The transformed optional.</returns>
        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Option<TResult>> mapping, Func<TException> exceptionFactory)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.FlatMap(mapping, exceptionFactory)));
        }

        public AsyncOption<TResult, TException> FlatMap<TResult>(Func<T, Task<TResult>> mapping)
        {
            return FlatMap(value => mapping(value).Map(result => Option.Some<TResult, TException>(result)));
        }

        /// <summary>
        /// Empties an optional, and attaches an exceptional value, 
        /// if a specified predicate is not satisfied.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="exception">The exceptional value to attach.</param>
        /// <returns>The filtered optional.</returns>
        public AsyncOption<T, TException> Filter(Func<T, bool> predicate, TException exception)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Filter(predicate, exception)));
        }

        /// <summary>
        /// Empties an optional, and attaches an exceptional value, 
        /// if a specified predicate is not satisfied.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="exceptionFactory">A factory function to create an exceptional value to attach.</param>
        /// <returns>The filtered optional.</returns>
        public AsyncOption<T, TException> Filter(Func<T, bool> predicate, Func<TException> exceptionFactory)
        {
            return AsyncOption.FromTask(optionTask.Map(option => option.Filter(predicate, exceptionFactory)));
        }
    }

    /// <summary>
    /// Provides a set of functions for creating asynchronous optional values.
    /// </summary>
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

        /// <summary>
        /// Wraps an existing value in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static AsyncOption<T> Some<T>(T value)
        {
            return new AsyncOption<T>(Task.FromResult(Option.Some<T>(value)));
        }

        /// <summary>
        /// Wraps an existing value in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static AsyncOption<T> Some<T>(Task<T> task)
        {
            return new AsyncOption<T>(task.Map(value => Option.Some(value)));
        }

        /// <summary>
        /// Creates an empty Option&lt;T&gt; instance.
        /// </summary>
        /// <returns>An empty optional.</returns>
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

        /// <summary>
        /// Wraps an existing value in an Option&lt;T, TException&gt; instance.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static AsyncOption<T, TException> Some<T, TException>(T value)
        {
            return new AsyncOption<T, TException>(Task.FromResult(Option.Some<T, TException>(value)));
        }

        /// <summary>
        /// Wraps an existing value in an Option&lt;T, TException&gt; instance.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static AsyncOption<T, TException> Some<T, TException>(Task<T> task)
        {
            return new AsyncOption<T, TException>(task.Map(value => Option.Some<T, TException>(value)));
        }

        /// <summary>
        /// Creates an empty Option&lt;T, TException&gt; instance, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="exception">The exceptional value.</param>
        /// <returns>An empty optional.</returns>
        public static AsyncOption<T, TException> None<T, TException>(TException exception)
        {
            return new AsyncOption<T, TException>(Task.FromResult(Option.None<T, TException>(exception)));
        }
    }
}
