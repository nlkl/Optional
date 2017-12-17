using System;
using System.Threading.Tasks;

namespace Optional.Async
{
    public static class OptionTaskExtensions
    {
        public static async Task<Option<TResult>> MapAsync<T, TResult>(this Option<T> option, Func<T, Task<TResult>> mapping)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            foreach (var valueTask in option.Map(mapping))
            {
                if (valueTask == null) throw new InvalidOperationException($"{nameof(mapping)} must not return a null task.");

                var value = await valueTask.ConfigureAwait(continueOnCapturedContext: false);
                return value.Some();
            }

            return Option.None<TResult>();
        }

        public static async Task<Option<TResult>> MapAsync<T, TResult>(this Task<Option<T>> optionTask, Func<T, TResult> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.Map(mapping);
        }

        public static async Task<Option<TResult>> MapAsync<T, TResult>(this Task<Option<T>> optionTask, Func<T, Task<TResult>> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.MapAsync(mapping).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static async Task<Option<TResult>> FlatMapAsync<T, TResult>(this Option<T> option, Func<T, Task<Option<TResult>>> mapping)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            foreach (var resultOptionTask in option.Map(mapping))
            {
                if (resultOptionTask == null) throw new InvalidOperationException($"{nameof(mapping)} must not return a null task.");
                return await resultOptionTask.ConfigureAwait(continueOnCapturedContext: false);
            }

            return Option.None<TResult>();
        }

        public static async Task<Option<TResult>> FlatMapAsync<T, TResult>(this Task<Option<T>> optionTask, Func<T, Option<TResult>> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.FlatMap(mapping);
        }

        public static async Task<Option<TResult>> FlatMapAsync<T, TResult>(this Task<Option<T>> optionTask, Func<T, Task<Option<TResult>>> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.FlatMapAsync(mapping).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static async Task<Option<TResult>> FlatMapAsync<T, TResult, TException>(this Option<T> option, Func<T, Task<Option<TResult, TException>>> mapping)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            foreach (var resultOptionTask in option.Map(mapping))
            {
                if (resultOptionTask == null) throw new InvalidOperationException($"{nameof(mapping)} must not return a null task.");
                var resultOption = await resultOptionTask.ConfigureAwait(continueOnCapturedContext: false);
                return resultOption.WithoutException();
            }

            return Option.None<TResult>();
        }

        public static async Task<Option<TResult>> FlatMapAsync<T, TResult, TException>(this Task<Option<T>> optionTask, Func<T, Option<TResult, TException>> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.FlatMap(mapping);
        }

        public static async Task<Option<TResult>> FlatMapAsync<T, TResult, TException>(this Task<Option<T>> optionTask, Func<T, Task<Option<TResult, TException>>> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.FlatMapAsync(mapping).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static async Task<Option<T>> FilterAsync<T>(this Option<T> option, Func<T, Task<bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (var value in option)
            {
                var predicateTask = predicate(value);
                if (predicateTask == null) throw new InvalidOperationException("Predicate must not return a null task.");

                var condition = await predicateTask.ConfigureAwait(continueOnCapturedContext: false);
                return option.Filter(condition);
            }

            return Option.None<T>();
        }

        public static async Task<Option<T>> FilterAsync<T>(this Task<Option<T>> optionTask, Func<T, bool> predicate, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.Filter(predicate);
        }


        public static async Task<Option<T>> FilterAsync<T>(this Task<Option<T>> optionTask, Func<T, Task<bool>> predicate, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.FilterAsync(predicate).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static Task<Option<T>> NotNullAsync<T>(this Task<Option<T>> optionTask)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            return optionTask.FilterAsync(value => value != null);
        }

        public static async Task<Option<T>> OrAsync<T>(this Option<T> option, Func<Task<T>> alternativeFactory)
        {
            if (alternativeFactory == null) throw new ArgumentNullException(nameof(alternativeFactory));

            if (option.HasValue) return option;

            var alternativeTask = alternativeFactory();
            if (alternativeTask == null) throw new InvalidOperationException($"{nameof(alternativeFactory)} must not return a null task.");

            var alternative = await alternativeTask.ConfigureAwait(continueOnCapturedContext: false);
            return alternative.Some();
        }

        public static async Task<Option<T>> OrAsync<T>(this Task<Option<T>> optionTask, Func<T> alternativeFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (alternativeFactory == null) throw new ArgumentNullException(nameof(alternativeFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.Or(alternativeFactory);
        }

        public static async Task<Option<T>> OrAsync<T>(this Task<Option<T>> optionTask, Func<Task<T>> alternativeFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (alternativeFactory == null) throw new ArgumentNullException(nameof(alternativeFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.OrAsync(alternativeFactory).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static async Task<Option<T>> ElseAsync<T>(this Option<T> option, Func<Task<Option<T>>> alternativeOptionFactory)
        {
            if (alternativeOptionFactory == null) throw new ArgumentNullException(nameof(alternativeOptionFactory));

            if (option.HasValue) return option;

            var alternativeOptionTask = alternativeOptionFactory();
            if (alternativeOptionTask == null) throw new InvalidOperationException($"{nameof(alternativeOptionFactory)} must not return a null task.");

            return await alternativeOptionTask.ConfigureAwait(continueOnCapturedContext: false);
        }

        public static async Task<Option<T>> ElseAsync<T>(this Task<Option<T>> optionTask, Func<Option<T>> alternativeOptionFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (alternativeOptionFactory == null) throw new ArgumentNullException(nameof(alternativeOptionFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.Else(alternativeOptionFactory);
        }

        public static async Task<Option<T>> ElseAsync<T>(this Task<Option<T>> optionTask, Func<Task<Option<T>>> alternativeOptionFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (alternativeOptionFactory == null) throw new ArgumentNullException(nameof(alternativeOptionFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.ElseAsync(alternativeOptionFactory).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static async Task<Option<T, TException>> WithExceptionAsync<T, TException>(this Option<T> option, Func<Task<TException>> exceptionFactory)
        {
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));

            foreach (var value in option)
            {
                return Option.Some<T, TException>(value);
            }

            var exceptionTask = exceptionFactory();
            if (exceptionFactory == null) throw new InvalidOperationException($"{nameof(exceptionFactory)} must not return a null task.");

            var exception = await exceptionTask.ConfigureAwait(continueOnCapturedContext: false);
            return Option.None<T, TException>(exception);
        }

        public static async Task<Option<T, TException>> WithExceptionAsync<T, TException>(this Task<Option<T>> optionTask, Func<TException> exceptionFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.WithException(exceptionFactory);
        }

        public static async Task<Option<T, TException>> WithExceptionAsync<T, TException>(this Task<Option<T>> optionTask, Func<Task<TException>> exceptionFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.WithExceptionAsync(exceptionFactory).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static async Task<Option<T>> FlattenAsync<T>(this Task<Option<Option<T>>> optionTask)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));

            var option = await optionTask.ConfigureAwait(continueOnCapturedContext: false);
            return option.Flatten();
        }
    }
}
