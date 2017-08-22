// Note: The implementation is closely inspired by the corefx source code for FirstOrDefault, etc.

using System;
using System.Collections.Generic;

namespace Optional.Collections
{
    public static class OptionEnumerableExtensions
    {
        /// <summary>
        /// Returns the first element of a sequence if such exists.
        /// </summary>
        /// <param name="source">The sequence to return the first element from.</param>
        /// <returns>An Option&lt;T&gt; instance containing the first element if present.</returns>
        public static Option<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            //if (source is IPartition<TSource> partition)
            //{
            //    return partition.TryGetFirst(out found);
            //}

            if (source is IList<TSource> list)
            {
                if (list.Count > 0)
                {
                    return list[0].Some();
                }
            }
            else
            {
                using (var enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        return enumerator.Current.Some();
                    }
                }
            }

            return Option.None<TSource>();
        }

        /// <summary>
        /// Returns the first element of a sequence, satisfying a specified predicate, 
        /// if such exists.
        /// </summary>
        /// <param name="source">The sequence to return the first element from.</param>
        /// <param name="predicate">The predicate to filter by.</param>
        /// <returns>An Option&lt;T&gt; instance containing the first element if present.</returns>
        public static Option<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            //if (source is OrderedEnumerable<TSource> ordered)
            //{
            //    return ordered.TryGetFirst(predicate, out found);
            //}

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return element.Some();
                }
            }

            return Option.None<TSource>();
        }

        /// <summary>
        /// Returns the last element of a sequence if such exists.
        /// </summary>
        /// <param name="source">The sequence to return the last element from.</param>
        /// <returns>An Option&lt;T&gt; instance containing the last element if present.</returns>
        public static Option<TSource> LastOrNone<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            //if (source is IPartition<TSource> partition)
            //{
            //    return partition.TryGetLast(out found);
            //}

            if (source is IList<TSource> list)
            {
                int count = list.Count;
                if (count > 0)
                {
                    return list[count - 1].Some();
                }
            }
            else
            {
                using (var enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        TSource result;
                        do
                        {
                            result = enumerator.Current;
                        }
                        while (enumerator.MoveNext());

                        return result.Some();
                    }
                }
            }

            return Option.None<TSource>();
        }

        /// <summary>
        /// Returns the last element of a sequence, satisfying a specified predicate, 
        /// if such exists.
        /// </summary>
        /// <param name="source">The sequence to return the last element from.</param>
        /// <param name="predicate">The predicate to filter by.</param>
        /// <returns>An Option&lt;T&gt; instance containing the last element if present.</returns>
        public static Option<TSource> LastOrNone<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            //if (source is OrderedEnumerable<TSource> ordered)
            //{
            //    return ordered.TryGetLast(predicate, out found);
            //}

            if (source is IList<TSource> list)
            {
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    var result = list[i];
                    if (predicate(result))
                    {
                        return result.Some();
                    }
                }
            }
            else
            {
                using (var enumerator = source.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var result = enumerator.Current;
                        if (predicate(result))
                        {
                            while (enumerator.MoveNext())
                            {
                                var element = enumerator.Current;
                                if (predicate(element))
                                {
                                    result = element;
                                }
                            }

                            return result.Some();
                        }
                    }
                }
            }

            return Option.None<TSource>();
        }

        /// <summary>
        /// Returns a single element from a sequence, if it exists 
        /// and is the only element in the sequence.
        /// </summary>
        /// <param name="source">The sequence to return the element from.</param>
        /// <returns>An Option&lt;T&gt; instance containing the element if present.</returns>
        public static Option<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source is IList<TSource> list)
            {
                switch (list.Count)
                {
                    case 0: return Option.None<TSource>();
                    case 1: return list[0].Some();
                }
            }
            else
            {
                using (var enumerator = source.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                    {
                        return Option.None<TSource>();
                    }

                    var result = enumerator.Current;
                    if (!enumerator.MoveNext())
                    {
                        return result.Some();
                    }
                }
            }

            return Option.None<TSource>();
        }

        /// <summary>
        /// Returns a single element from a sequence, satisfying a specified predicate, 
        /// if it exists and is the only element in the sequence.
        /// </summary>
        /// <param name="source">The sequence to return the element from.</param>
        /// <param name="predicate">The predicate to filter by.</param>
        /// <returns>An Option&lt;T&gt; instance containing the element if present.</returns>
        public static Option<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var result = enumerator.Current;
                    if (predicate(result))
                    {
                        while (enumerator.MoveNext())
                        {
                            if (predicate(enumerator.Current))
                            {
                                return Option.None<TSource>();
                            }
                        }

                        return result.Some();
                    }
                }
            }

            return Option.None<TSource>();
        }

        /// <summary>
        /// Returns an element at a specified position in a sequence if such exists.
        /// </summary>
        /// <param name="source">The sequence to return the element from.</param>
        /// <param name="index">The index in the sequence.</param>
        /// <returns>An Option&lt;T&gt; instance containing the element if found.</returns>
        public static Option<TSource> ElementAtOrNone<TSource>(this IEnumerable<TSource> source, int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (index >= 0)
            {
                if (source is IList<TSource> list)
                {
                    if (index < list.Count)
                    {
                        return list[index].Some();
                    }
                }
                else
                {
                    using (var enumerator = source.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (index == 0)
                            {
                                return enumerator.Current.Some();
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
