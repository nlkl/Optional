using Optional.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions.Collections
{
    public static class ParallelEnumerableEnumerableLinqExtensions
    {
        public static Option<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> source)
        {
            throw new NotImplementedException();
        }

        public static Option<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public static Option<TSource> LastOrNone<TSource>(this IEnumerable<TSource> source)
        {
            throw new NotImplementedException();
        }

        public static Option<TSource> LastOrNone<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public static Option<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> source)
        {
            throw new NotImplementedException();
        }

        public static Option<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public static Option<TSource> ElementAtOrNone<TSource>(this IEnumerable<TSource> source, int index)
        {
            throw new NotImplementedException();
        }
    }
}
