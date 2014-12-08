using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optional
{
    public struct Option<T>
    {
        private bool hasValue;
        private T value;

        /// <summary>
        /// Checks if a value is present.
        /// </summary>
        public bool HasValue { get { return hasValue; } }
        internal T Value { get { return value; } }

        internal Option(T value, bool hasValue)
        {
            this.value = value;
            this.hasValue = hasValue;
        }
    }

    public static class Option
    {
        /// <summary>
        /// Wraps an existing value in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns>An Option&lt;T&gt; instance containing the specified value.</returns>
        public static Option<T> Some<T>(T value)
        {
            return new Option<T>(value, true);
        }

        /// <summary>
        /// Creates an empty Option&lt;T&gt; instance.
        /// </summary>
        /// <returns>An empty instance of Option&lt;T&gt;.</returns>
        public static Option<T> None<T>()
        {
            return new Option<T>(default(T), false);
        }
    }
}
