using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optional.Extensions.Collections
{
    public static class OptionEnumerableExtensions
    {
        /// <summary>
        /// Converts an Option&lt;T&gt; instance to an enumerable with one or zero elements.
        /// </summary>
        /// <param name="option">The Option&lt;T&gt; instance.</param>
        /// <returns>A corresponding enumerable.</returns>
        public static IEnumerable<T> ToEnumerable<T>(this Option<T> option)
        {
            return option
                .Map(value => Enumerable.Repeat(value, 1))
                .ValueOr(Enumerable.Empty<T>());
        }
    }
}
