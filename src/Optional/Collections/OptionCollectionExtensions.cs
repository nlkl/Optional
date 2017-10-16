// Note: Several of the below implementations are closely inspired by the corefx source code for FirstOrDefault, etc.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Optional.Collections
{
    public static class OptionCollectionExtensions
    {

        /// <summary>
        /// Flattens a sequence of optionals into a sequence containing all inner values.
        /// Empty elements are discarded.
        /// </summary>
        /// <param name="source">The sequence of optionals.</param>
        /// <returns>A flattened sequence of values.</returns>
        public static IEnumerable<T> Values<T>(this IEnumerable<Option<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var option in source)
            {
                if (option.HasValue)
                {
                    yield return option.Value;
                }
            }
        }

        /// <summary>
        /// Flattens a sequence of optionals into a sequence containing all inner values.
        /// Empty elements and their exceptional values are discarded.
        /// </summary>
        /// <param name="source">The sequence of optionals.</param>
        /// <returns>A flattened sequence of values.</returns>
        public static IEnumerable<T> Values<T, TException>(this IEnumerable<Option<T, TException>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var option in source)
            {
                if (option.HasValue)
                {
                    yield return option.Value;
                }
            }
        }

        /// <summary>
        /// Flattens a sequence of optionals into a sequence containing all exceptional values.
        /// Non-empty elements and their values are discarded.
        /// </summary>
        /// <param name="source">The sequence of optionals.</param>
        /// <returns>A flattened sequence of exceptional values.</returns>
        public static IEnumerable<TException> Exceptions<T, TException>(this IEnumerable<Option<T, TException>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var option in source)
            {
                if (!option.HasValue)
                {
                    yield return option.Exception;
                }
            }
        }

        /// <summary>
        /// Returns the value associated with the specified key if such exists.
        /// A dictionary lookup will be used if available, otherwise falling
        /// back to a linear scan of the enumerable.
        /// </summary>
        /// <param name="source">The dictionary or enumerable in which to locate the key.</param>
        /// <param name="key">The key to locate.</param>
        /// <returns>An Option&lt;TValue&gt; instance containing the associated value if located.</returns>
        public static Option<TValue> GetValueOrNone<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, TKey key)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source is IDictionary<TKey, TValue> dictionary)
            {
                return dictionary.TryGetValue(key, out var value) ? value.Some() : value.None();
            }
#if !NET35
            else if (source is IReadOnlyDictionary<TKey, TValue> readOnlyDictionary)
            {
                return readOnlyDictionary.TryGetValue(key, out var value) ? value.Some() : value.None();
            }
#endif

            return source
                .FirstOrNone(pair => EqualityComparer<TKey>.Default.Equals(pair.Key, key))
                .Map(pair => pair.Value);
        }

        /// <summary>
        /// Returns the first element of a sequence if such exists.
        /// </summary>
        /// <param name="source">The sequence to return the first element from.</param>
        /// <returns>An Option&lt;T&gt; instance containing the first element if present.</returns>
        public static Option<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source is IList<TSource> list)
            {
                if (list.Count > 0)
                {
                    return list[0].Some();
                }
            }
#if !NET35
            else if (source is IReadOnlyList<TSource> readOnlyList)
            {
                if (readOnlyList.Count > 0)
                {
                    return readOnlyList[0].Some();
                }
            }
#endif
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

            if (source is IList<TSource> list)
            {
                var count = list.Count;
                if (count > 0)
                {
                    return list[count - 1].Some();
                }
            }
#if !NET35
            else if (source is IReadOnlyList<TSource> readOnlyList)
            {
                var count = readOnlyList.Count;
                if (count > 0)
                {
                    return readOnlyList[count - 1].Some();
                }
            }
#endif
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

            if (source is IList<TSource> list)
            {
                for (var i = list.Count - 1; i >= 0; --i)
                {
                    var result = list[i];
                    if (predicate(result))
                    {
                        return result.Some();
                    }
                }
            }
#if !NET35
            else if (source is IReadOnlyList<TSource> readOnlyList)
            {
                for (var i = readOnlyList.Count - 1; i >= 0; --i)
                {
                    var result = readOnlyList[i];
                    if (predicate(result))
                    {
                        return result.Some();
                    }
                }
            }
#endif
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
#if !NET35
            else if (source is IReadOnlyList<TSource> readOnlyList)
            {
                switch (readOnlyList.Count)
                {
                    case 0: return Option.None<TSource>();
                    case 1: return readOnlyList[0].Some();
                }
            }
#endif
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
#if !NET35
                else if (source is IReadOnlyList<TSource> readOnlyList)
                {
                    if (index < readOnlyList.Count)
                    {
                        return readOnlyList[index].Some();
                    }
                }
#endif
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
