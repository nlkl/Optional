using System;
using System.Threading.Tasks;

namespace Optional.Async.Linq
{
    public static class OptionTaskLinqExtensions
    {
        public static Task<Option<TResult>> Select<TSource, TResult>(this Task<Option<TSource>> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return source.MapAsync(selector);
        }

        public static Task<Option<TResult>> SelectMany<TSource, TResult>(this Task<Option<TSource>> source, Func<TSource, Task<Option<TResult>>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return source.FlatMapAsync(selector);
        }

        public static Task<Option<TResult>> SelectMany<TSource, TCollection, TResult>(
                this Task<Option<TSource>> source,
                Func<TSource, Task<Option<TCollection>>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null) throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            return source.FlatMapAsync(src => collectionSelector(src).MapAsync(elem => resultSelector(src, elem)));
        }

        public static Task<Option<TSource>> Where<TSource>(this Task<Option<TSource>> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return source.FilterAsync(predicate);
        }

        public static Task<Option<TResult, TException>> Select<TSource, TException, TResult>(this Task<Option<TSource, TException>> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return source.MapAsync(selector);
        }

        public static Task<Option<TResult, TException>> SelectMany<TSource, TException, TResult>(
                this Task<Option<TSource, TException>> source,
                Func<TSource, Task<Option<TResult, TException>>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return source.FlatMapAsync(selector);
        }

        public static Task<Option<TResult, TException>> SelectMany<TSource, TException, TCollection, TResult>(
                this Task<Option<TSource, TException>> source,
                Func<TSource, Task<Option<TCollection, TException>>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null) throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            return source.FlatMapAsync(src => collectionSelector(src).MapAsync(elem => resultSelector(src, elem)));
        }
    }
}
