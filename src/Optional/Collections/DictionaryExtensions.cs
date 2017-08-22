using System;
using System.Collections.Generic;

namespace Optional.Collections
{
    public static class DictionaryExtensions
    {
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
#if NET45PLUS
            else if (source is IReadOnlyDictionary<TKey, TValue> readOnlyDictionary)
            {
                return readOnlyDictionary.TryGetValue(key, out var value) ? value.Some() : value.None();
            }
#endif

            return source
                .FirstOrNone(pair => EqualityComparer<TKey>.Default.Equals(pair.Key, key))
                .Map(pair => pair.Value);
        }
    }
}
