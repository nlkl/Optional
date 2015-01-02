using Optional.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions.Collections
{
    public static class EnumerableEnumerableLinqExtensions
    {
        public static Option<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> source)
        {
            Guard.NotNull(source, "source");

            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                if (list.Count > 0)
                {
                    return list[0].Some();
                }
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        return e.Current.Some();
                    }
                }
            }

            return Option.None<TSource>();
        }

        public static Option<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(predicate, "predicate");

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return element.Some();
                }
            }

            return Option.None<TSource>();
        }

        public static Option<TSource> LastOrNone<TSource>(this IEnumerable<TSource> source)
        {
            Guard.NotNull(source, "source");

            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                int count = list.Count;
                if (count > 0)
                {
                    return list[count - 1].Some();
                }
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        TSource result;
                        do
                        {
                            result = e.Current;
                        } while (e.MoveNext());

                        return result.Some();
                    }
                }
            }

            return Option.None<TSource>();
        }

        public static Option<TSource> LastOrNone<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(predicate, "predicate");

            TSource result = default(TSource);
            bool exists = false;
            foreach (TSource element in source)
            {
                if (predicate(element))
                {
                    exists = true;
                    result = element;
                }
            }

            return exists ? result.Some() : result.None();
        }

        public static Option<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> source)
        {
            Guard.NotNull(source, "source");

            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                if (list.Count == 1)
                {
                    return list[0].Some();
                }
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (!e.MoveNext())
                    {
                        return Option.None<TSource>();
                    }

                    TSource result = e.Current;

                    if (!e.MoveNext())
                    {
                        return result.Some();
                    }
                }
            }

            return Option.None<TSource>();
        }

        public static Option<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(predicate, "predicate");

            TSource result = default(TSource);
            long count = 0;
            foreach (TSource element in source)
            {
                if (predicate(element))
                {
                    result = element;
                    checked { count++; }

                    if (count > 1)
                    {
                        return result.None();
                    }
                }
            }

            return count == 1 ? result.Some() : result.None();
        }

        public static Option<TSource> ElementAtOrNone<TSource>(this IEnumerable<TSource> source, int index)
        {
            Guard.NotNull(source, "source");

            if (index >= 0)
            {
                IList<TSource> list = source as IList<TSource>;
                if (list != null)
                {
                    if (index < list.Count)
                    {
                        return list[index].Some();
                    }
                }
                else
                {
                    using (IEnumerator<TSource> e = source.GetEnumerator())
                    {
                        while (true)
                        {
                            if (!e.MoveNext())
                            {
                                break;
                            }

                            if (index == 0)
                            {
                                return e.Current.Some();
                            }

                            index--;
                        }
                    }
                }
            }

            return Option.None<TSource>();
        }
    }
}
