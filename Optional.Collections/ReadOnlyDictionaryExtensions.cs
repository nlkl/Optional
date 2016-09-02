using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Collections
{
    /// <summary>
    /// Provides extension methods using Option for IReadOnlyDictionary.
    /// </summary>
    public static class ReadOnlyDictionaryExtensions
    {
        /// <summary>
        /// Returns the value corresponding to the key if such exists.
        /// </summary>
        /// <param name="dictionary">The dictionary to lookup from.</param>
        /// <param name="key">The key to lookup.</param>
        /// <returns>An Option&lt;TValue&gt; instance containing the value corresponding to the key if present.</returns>
        public static Option<TValue> GetOrNone<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value)
                ? Option.Some(value)
                : Option.None<TValue>();
        }
    }
}
