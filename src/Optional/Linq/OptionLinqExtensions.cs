using System;

namespace Optional.Linq
{
    public static class OptionLinqExtensions
    {
        public static Option<TResult> Select<TSource, TResult>(this Option<TSource> source, Func<TSource, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return source.Map(selector);
        }

        public static Option<TResult> SelectMany<TSource, TResult>(this Option<TSource> source, Func<TSource, Option<TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return source.FlatMap(selector);
        }

        public static Option<TResult> SelectMany<TSource, TCollection, TResult>(
                this Option<TSource> source,
                Func<TSource, Option<TCollection>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector)
        {
            if (collectionSelector == null) throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            return source.FlatMap(src => collectionSelector(src).Map(elem => resultSelector(src, elem)));
        }

        public static Option<TSource> Where<TSource>(this Option<TSource> source, Func<TSource, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return source.Filter(predicate);
        }

        public static Option<TResult, TException> Select<TSource, TException, TResult>(this Option<TSource, TException> source, Func<TSource, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return source.Map(selector);
        }

        public static Option<TResult, TException> SelectMany<TSource, TException, TResult>(
                this Option<TSource, TException> source,
                Func<TSource, Option<TResult, TException>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return source.FlatMap(selector);
        }

        public static Option<TResult, TException> SelectMany<TSource, TException, TCollection, TResult>(
                this Option<TSource, TException> source,
                Func<TSource, Option<TCollection, TException>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector)
        {
            if (collectionSelector == null) throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            return source.FlatMap(src => collectionSelector(src).Map(elem => resultSelector(src, elem)));
        }
    }
}
