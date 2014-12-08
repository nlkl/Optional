using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optional.Linq
{
    public static class OptionLinqExtensions
    {
        public static Option<TResult> Select<TSource, TResult>(this Option<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Map(selector);
        }

        public static Option<TResult> SelectMany<TSource, TResult>(this Option<TSource> source, Func<TSource, Option<TResult>> selector)
        {
            return source.FlatMap(selector);
        }

        public static Option<TResult> SelectMany<TSource, TCollection, TResult>(
            this Option<TSource> source,
            Func<TSource, Option<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            return source.FlatMap(src => collectionSelector(src).Map(elem => resultSelector(src, elem)));
        }

        public static Option<TSource> Where<TSource>(this Option<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Filter(predicate);
        }
    }
}
