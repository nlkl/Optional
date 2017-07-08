using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Async
{
    public static class AsyncOptionExtensions
    {
        /// <summary>
        /// Creates an async optional.
        /// </summary>
        /// <param name="option">The source to construct the async optional from.</param>
        /// <returns>The async optional.</returns>
        public static AsyncOption<T> ToAsyncOption<T>(this Option<T> option)
        {
            return new AsyncOption<T>(Task.FromResult(option));
        }

        /// <summary>
        /// Creates an async optional.
        /// </summary>
        /// <param name="optionTask">The source to construct the async optional from.</param>
        /// <returns>The async optional.</returns>
        public static AsyncOption<T> ToAsyncOption<T>(this Task<Option<T>> optionTask)
        {
            return new AsyncOption<T>(optionTask);
        }

        /// <summary>
        /// Creates an async optional.
        /// </summary>
        /// <param name="option">The source to construct the async optional from.</param>
        /// <returns>The async optional.</returns>
        public static AsyncOption<T, TException> ToAsyncOption<T, TException>(this Option<T, TException> option)
        {
            return new AsyncOption<T, TException>(Task.FromResult(option));
        }

        /// <summary>
        /// Creates an async optional.
        /// </summary>
        /// <param name="optionTask">The source to construct the async optional from.</param>
        /// <returns>The async optional.</returns>
        public static AsyncOption<T, TException> ToAsyncOption<T, TException>(this Task<Option<T, TException>> optionTask)
        {
            return new AsyncOption<T, TException>(optionTask);
        }

        /// <summary>
        /// Returns the existing value if present, or the attached 
        /// exceptional value.
        /// </summary>
        /// <param name="option">The specified async optional.</param>
        /// <returns>The existing or exceptional value.</returns>
        public static Task<T> ValueOrException<T>(this AsyncOption<T, T> option)
        {
            return option.Match(value => value, exception => exception);
        }

        // TODO: Document and test
        public static AsyncOption<TResult> Map<T, TResult>(this Option<T> option, Func<T, Task<TResult>> mapping)
        {
            return option.ToAsyncOption().Map(mapping);
        }

        // TODO: Document and test
        public static AsyncOption<TResult> FlatMap<T, TResult>(this Option<T> option, Func<T, Task<Option<TResult>>> mapping)
        {
            return option.ToAsyncOption().FlatMap(mapping);
        }

        // TODO: Document and test
        public static AsyncOption<TResult> FlatMap<T, TResult>(this Option<T> option, Func<T, AsyncOption<TResult>> mapping)
        {
            return option.ToAsyncOption().FlatMap(mapping);
        }

        // TODO: Document and test
        public static AsyncOption<TResult> FlatMap<T, TResult, TException>(this Option<T> option, Func<T, Task<Option<TResult, TException>>> mapping)
        {
            return option.ToAsyncOption().FlatMap(mapping);
        }

        // TODO: Document and test
        public static AsyncOption<TResult> FlatMap<T, TResult, TException>(this Option<T> option, Func<T, AsyncOption<TResult, TException>> mapping)
        {
            return option.ToAsyncOption().FlatMap(mapping);
        }

        // TODO: Document and test
        public static AsyncOption<TResult, TException> Map<T, TException, TResult>(this Option<T, TException> option, Func<T, Task<TResult>> mapping)
        {
            return option.ToAsyncOption().Map(mapping);
        }

        // TODO: Document and test
        public static AsyncOption<T, TExceptionResult> MapException<T, TException, TExceptionResult>(this Option<T, TException> option, Func<TException, Task<TExceptionResult>> mapping)
        {
            return option.ToAsyncOption().MapException(mapping);
        }

        // TODO: Document and test
        public static AsyncOption<TResult, TException> FlatMap<T, TException, TResult>(this Option<T, TException> option, Func<T, Task<Option<TResult, TException>>> mapping)
        {
            return option.ToAsyncOption().FlatMap(mapping);
        }

        // TODO: Document and test
        public static AsyncOption<TResult, TException> FlatMap<T, TException, TResult>(this Option<T, TException> option, Func<T, Task<Option<TResult>>> mapping, TException exception)
        {
            return option.ToAsyncOption().FlatMap(mapping, exception);
        }

        // TODO: Document and test
        public static AsyncOption<TResult, TException> FlatMap<T, TException, TResult>(this Option<T, TException> option, Func<T, Task<Option<TResult>>> mapping, Func<TException> exceptionFactory)
        {
            return option.ToAsyncOption().FlatMap(mapping, exceptionFactory);
        }

        // TODO: Document and test
        public static AsyncOption<TResult, TException> FlatMap<T, TException, TResult>(this Option<T, TException> option, Func<T, AsyncOption<TResult, TException>> mapping)
        {
            return option.ToAsyncOption().FlatMap(mapping);
        }

        // TODO: Document and test
        public static AsyncOption<TResult, TException> FlatMap<T, TException, TResult>(this Option<T, TException> option, Func<T, AsyncOption<TResult>> mapping, TException exception)
        {
            return option.ToAsyncOption().FlatMap(mapping, exception);
        }

        // TODO: Document and test
        public static AsyncOption<TResult, TException> FlatMap<T, TException, TResult>(this Option<T, TException> option, Func<T, AsyncOption<TResult>> mapping, Func<TException> exceptionFactory)
        {
            return option.ToAsyncOption().FlatMap(mapping, exceptionFactory);
        }
    }
}
