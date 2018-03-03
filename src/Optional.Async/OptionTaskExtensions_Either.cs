using System;
using System.Threading.Tasks;

namespace Optional.Async
{
    public static partial class OptionTaskExtensions
    {
        public static Task<Option<TResult, TException>> MapAsync<T, TException, TResult>(this Option<T, TException> option, Func<T, Task<TResult>> mapping)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            return option.Map(mapping).Match(
                some: async valueTask =>
                {
                    if (valueTask == null) throw new InvalidOperationException($"{nameof(mapping)} must not return a null task.");
                    var value = await valueTask.ConfigureAwait(continueOnCapturedContext: false);
                    return value.Some<TResult, TException>();
                },
                none: exception => Task.FromResult(Option.None<TResult, TException>(exception))
            );
        }

        public static async Task<Option<TResult, TException>> MapAsync<T, TException, TResult>(this Task<Option<T, TException>> optionTask, Func<T, TResult> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.Map(mapping);
        }

        public static async Task<Option<TResult, TException>> MapAsync<T, TException, TResult>(this Task<Option<T, TException>> optionTask, Func<T, Task<TResult>> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.MapAsync(mapping).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static Task<Option<T, TExceptionResult>> MapExceptionAsync<T, TException, TExceptionResult>(this Option<T, TException> option, Func<TException, Task<TExceptionResult>> mapping)
        {
            return option.MapException(mapping).Match(
                some: value => Task.FromResult(Option.Some<T, TExceptionResult>(value)),
                none: async exceptionTask =>
                {
                    if (exceptionTask == null) throw new InvalidOperationException($"{nameof(mapping)} must not return a null task.");
                    var exception = await exceptionTask.ConfigureAwait(continueOnCapturedContext: false);
                    return Option.None<T, TExceptionResult>(exception);
                }
            );
        }

        public static async Task<Option<T, TExceptionResult>> MapExceptionAsync<T, TException, TExceptionResult>(this Task<Option<T, TException>> optionTask, Func<TException, TExceptionResult> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.MapException(mapping);
        }

        public static async Task<Option<T, TExceptionResult>> MapExceptionAsync<T, TException, TExceptionResult>(this Task<Option<T, TException>> optionTask, Func<TException, Task<TExceptionResult>> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.MapExceptionAsync(mapping).ConfigureAwait(false);
        }

        public static Task<Option<TResult, TException>> FlatMapAsync<T, TException, TResult>(this Option<T, TException> option, Func<T, Task<Option<TResult, TException>>> mapping)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            return option.Map(mapping).Match(
                some: resultOptionTask =>
                {
                    if (resultOptionTask == null) throw new InvalidOperationException($"{nameof(mapping)} must not return a null task.");
                    return resultOptionTask;
                },
                none: exception => Task.FromResult(Option.None<TResult, TException>(exception))
            );
        }

        public static async Task<Option<TResult, TException>> FlatMapAsync<T, TException, TResult>(this Task<Option<T, TException>> optionTask, Func<T, Option<TResult, TException>> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.FlatMap(mapping);
        }

        public static async Task<Option<TResult, TException>> FlatMapAsync<T, TException, TResult>(this Task<Option<T, TException>> optionTask, Func<T, Task<Option<TResult, TException>>> mapping, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.FlatMapAsync(mapping).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static Task<Option<TResult, TException>> FlatMapAsync<T, TException, TResult>(this Option<T, TException> option, Func<T, Task<Option<TResult>>> mapping, TException exception) =>
            option.FlatMapAsync(mapping, () => exception);

        public static Task<Option<TResult, TException>> FlatMapAsync<T, TException, TResult>(this Option<T, TException> option, Func<T, Task<Option<TResult>>> mapping, Func<TException> exceptionFactory)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));

            return option.FlatMapAsync(async value =>
            {
                var resultOptionTask = mapping(value) ?? throw new InvalidOperationException($"{nameof(mapping)} must not return a null task.");
                var resultOption = await (resultOptionTask).ConfigureAwait(false);
                return resultOption.WithException(exceptionFactory());
            });
        }

        public static Task<Option<TResult, TException>> FlatMapAsync<T, TException, TResult>(this Task<Option<T, TException>> optionTask, Func<T, Option<TResult>> mapping, TException exception, bool executeOnCapturedContext = false) =>
            optionTask.FlatMapAsync(mapping, () => exception, executeOnCapturedContext);

        public static async Task<Option<TResult, TException>> FlatMapAsync<T, TException, TResult>(this Task<Option<T, TException>> optionTask, Func<T, Option<TResult>> mapping, Func<TException> exceptionFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.FlatMap(mapping, exceptionFactory);
        }

        public static Task<Option<TResult, TException>> FlatMapAsync<T, TException, TResult>(this Task<Option<T, TException>> optionTask, Func<T, Task<Option<TResult>>> mapping, TException exception, bool executeOnCapturedContext = false) =>
            optionTask.FlatMapAsync(mapping, () => exception, executeOnCapturedContext);

        public static async Task<Option<TResult, TException>> FlatMapAsync<T, TException, TResult>(this Task<Option<T, TException>> optionTask, Func<T, Task<Option<TResult>>> mapping, Func<TException> exceptionFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.FlatMapAsync(mapping, exceptionFactory).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static Task<Option<T, TException>> FilterAsync<T, TException>(this Option<T, TException> option, Func<T, Task<bool>> predicate, TException exception) =>
            option.FilterAsync(predicate, () => exception);

        public static Task<Option<T, TException>> FilterAsync<T, TException>(this Option<T, TException> option, Func<T, Task<bool>> predicate, Func<TException> exceptionFactory)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));

            return option.Match(
                some: async value =>
                {
                    var predicateTask = predicate(value);
                    if (predicateTask == null) throw new InvalidOperationException($"{nameof(predicate)} must not return a null task.");

                    var condition = await predicateTask.ConfigureAwait(continueOnCapturedContext: false);
                    return option.Filter(condition, exceptionFactory);
                },
                none: _ => Task.FromResult(option)
            );
        }

        public static Task<Option<T, TException>> FilterAsync<T, TException>(this Task<Option<T, TException>> optionTask, Func<T, bool> predicate, TException exception, bool executeOnCapturedContext = false) =>
            optionTask.FilterAsync(predicate, () => exception, executeOnCapturedContext);

        public static async Task<Option<T, TException>> FilterAsync<T, TException>(this Task<Option<T, TException>> optionTask, Func<T, bool> predicate, Func<TException> exceptionFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.Filter(predicate, exceptionFactory);
        }

        public static Task<Option<T, TException>> FilterAsync<T, TException>(this Task<Option<T, TException>> optionTask, Func<T, Task<bool>> predicate, TException exception, bool executeOnCapturedContext = false) =>
            optionTask.FilterAsync(predicate, () => exception, executeOnCapturedContext);

        public static async Task<Option<T, TException>> FilterAsync<T, TException>(this Task<Option<T, TException>> optionTask, Func<T, Task<bool>> predicate, Func<TException> exceptionFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.FilterAsync(predicate, exceptionFactory).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static Task<Option<T, TException>> NotNullAsync<T, TException>(this Task<Option<T, TException>> optionTask, TException exception) =>
            optionTask.NotNullAsync(() => exception);

        public static Task<Option<T, TException>> NotNullAsync<T, TException>(this Task<Option<T, TException>> optionTask, Func<TException> exceptionFactory)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));
            return optionTask.FilterAsync(value => value != null, exceptionFactory);
        }

        public static async Task<Option<T, TException>> OrAsync<T, TException>(this Option<T, TException> option, Func<Task<T>> alternativeFactory)
        {
            if (alternativeFactory == null) throw new ArgumentNullException(nameof(alternativeFactory));

            if (option.HasValue) return option;

            var alternativeTask = alternativeFactory();
            if (alternativeTask == null) throw new InvalidOperationException($"{nameof(alternativeFactory)} must not return a null task.");

            var alternative = await alternativeTask.ConfigureAwait(continueOnCapturedContext: false);
            return alternative.Some<T, TException>();
        }

        public static async Task<Option<T, TException>> OrAsync<T, TException>(this Task<Option<T, TException>> optionTask, Func<T> alternativeFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (alternativeFactory == null) throw new ArgumentNullException(nameof(alternativeFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.Or(alternativeFactory);
        }

        public static async Task<Option<T, TException>> OrAsync<T, TException>(this Task<Option<T, TException>> optionTask, Func<Task<T>> alternativeFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (alternativeFactory == null) throw new ArgumentNullException(nameof(alternativeFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.OrAsync(alternativeFactory).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static async Task<Option<T, TException>> ElseAsync<T, TException>(this Option<T, TException> option, Func<Task<Option<T, TException>>> alternativeOptionFactory)
        {
            if (alternativeOptionFactory == null) throw new ArgumentNullException(nameof(alternativeOptionFactory));

            if (option.HasValue) return option;

            var alternativeOptionTask = alternativeOptionFactory();
            if (alternativeOptionTask == null) throw new InvalidOperationException($"{nameof(alternativeOptionFactory)} must not return a null task.");

            return await alternativeOptionTask.ConfigureAwait(continueOnCapturedContext: false);
        }

        public static async Task<Option<T, TException>> ElseAsync<T, TException>(this Task<Option<T, TException>> optionTask, Func<Option<T, TException>> alternativeOptionFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (alternativeOptionFactory == null) throw new ArgumentNullException(nameof(alternativeOptionFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return option.Else(alternativeOptionFactory);
        }

        public static async Task<Option<T, TException>> ElseAsync<T, TException>(this Task<Option<T, TException>> optionTask, Func<Task<Option<T, TException>>> alternativeOptionFactory, bool executeOnCapturedContext = false)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));
            if (alternativeOptionFactory == null) throw new ArgumentNullException(nameof(alternativeOptionFactory));

            var option = await optionTask.ConfigureAwait(executeOnCapturedContext);
            return await option.ElseAsync(alternativeOptionFactory).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static async Task<Option<T>> WithoutExceptionAsync<T, TException>(this Task<Option<T, TException>> optionTask)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));

            var option = await optionTask.ConfigureAwait(false);
            return option.WithoutException();
        }

        public static async Task<Option<T, TException>> FlattenAsync<T, TException>(this Task<Option<Option<T, TException>, TException>> optionTask)
        {
            if (optionTask == null) throw new ArgumentNullException(nameof(optionTask));

            var option = await optionTask.ConfigureAwait(continueOnCapturedContext: false);
            return option.Flatten();
        }
    }
}
