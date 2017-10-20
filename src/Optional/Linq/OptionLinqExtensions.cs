﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional.Internals;

namespace Optional.Linq
{
    public static class OptionLinqExtensions
    {
        public static Option<TResult> Select<TSource, TResult>(this Option<TSource> source, Func<TSource, TResult> selector)
        {
            Guard.ArgumentNotNull(selector);

            return source.Map(selector);
        }

        public static Option<TResult> SelectMany<TSource, TResult>(this Option<TSource> source, Func<TSource, Option<TResult>> selector)
        {
            Guard.ArgumentNotNull(selector);

            return source.FlatMap(selector);
        }

        public static Option<TResult> SelectMany<TSource, TCollection, TResult>(
                this Option<TSource> source,
                Func<TSource, Option<TCollection>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector)
        {
            Guard.ArgumentsNotNull(collectionSelector, resultSelector);

            return source.FlatMap(src => collectionSelector(src).Map(elem => resultSelector(src, elem)));
        }

        public static Option<TSource> Where<TSource>(this Option<TSource> source, Func<TSource, bool> predicate)
        {
            Guard.ArgumentNotNull(predicate);

            return source.Filter(predicate);
        }

        public static Option<TResult, TException> Select<TSource, TException, TResult>(this Option<TSource, TException> source, Func<TSource, TResult> selector)
        {
            Guard.ArgumentNotNull(selector);

            return source.Map(selector);
        }

        public static Option<TResult, TException> SelectMany<TSource, TException, TResult>(
                this Option<TSource, TException> source,
                Func<TSource,
                Option<TResult, TException>> selector)
        {
            Guard.ArgumentNotNull(selector);

            return source.FlatMap(selector);
        }

        public static Option<TResult, TException> SelectMany<TSource, TException, TCollection, TResult>(
                this Option<TSource, TException> source,
                Func<TSource, Option<TCollection, TException>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector)
        {
            Guard.ArgumentsNotNull(collectionSelector, resultSelector);

            return source.FlatMap(src => collectionSelector(src).Map(elem => resultSelector(src, elem)));
        }
    }
}
