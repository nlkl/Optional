using Optional.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions.Collections
{
    public static class QueryableLinqExtensions
    {
        public static Option<TSource> FirstOrNone<TSource>(this IQueryable<TSource> source)
        {
            Guard.NotNull(source, "source");

            var result = source.Select(val => new { Value = val }).FirstOrDefault();
            return result != null ? result.Value.Some() : Option.None<TSource>();
        }

        public static Option<TSource> FirstOrNone<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(predicate, "predicate");

            var result = source.Where(predicate).Select(val => new { Value = val }).FirstOrDefault();
            return result != null ? result.Value.Some() : Option.None<TSource>();
        }

        public static Option<TSource> LastOrNone<TSource>(this IQueryable<TSource> source)
        {
            Guard.NotNull(source, "source");

            var result = source.Select(val => new { Value = val }).LastOrDefault();
            return result != null ? result.Value.Some() : Option.None<TSource>();
        }

        public static Option<TSource> LastOrNone<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(predicate, "predicate");

            var result = source.Where(predicate).Select(val => new { Value = val }).LastOrDefault();
            return result != null ? result.Value.Some() : Option.None<TSource>();
        }

        public static Option<TSource> SingleOrNone<TSource>(this IQueryable<TSource> source)
        {
            Guard.NotNull(source, "source");

            try
            {
                var result = source.Select(val => new { Value = val }).SingleOrDefault();
                return result != null ? result.Value.Some() : Option.None<TSource>();
            }
            catch (InvalidOperationException)
            {
                return Option.None<TSource>();
            }
        }

        public static Option<TSource> SingleOrNone<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            Guard.NotNull(source, "source");

            try
            {
                var result = source.Where(predicate).Select(val => new { Value = val }).SingleOrDefault();
                return result != null ? result.Value.Some() : Option.None<TSource>();
            }
            catch (InvalidOperationException)
            {
                return Option.None<TSource>();
            }
        }

        public static Option<TSource> ElementAtOrNone<TSource>(this IQueryable<TSource> source, int index)
        {
            Guard.NotNull(source, "source");

            var result = source.Select(val => new { Value = val }).ElementAtOrDefault(index);
            return result != null ? result.Value.Some() : Option.None<TSource>();
        }
    }
}
