using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optional
{
    public static class OptionExtensions
    {
        /// <summary>
        /// Wraps a value in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The Option&lt;T&gt; instance.</returns>
        public static Option<T> Some<T>(this T value)
        {
            return Option.Some(value);
        }

        /// <summary>
        /// Creates an empty Option&lt;T&gt; instance from a specified value.
        /// </summary>
        /// <param name="value">A value determining the type of the Option&lt;T&gt; instance.</param>
        /// <returns>The Option&lt;T&gt; instance.</returns>
        public static Option<T> None<T>(this T value)
        {
            return Option.None<T>();
        }

        public static Option<T, TOther> Some<T, TOther>(this T value)
        {
            return Option.Some<T, TOther>(value);
        }

        public static Option<T, TOther> Other<T, TOther>(this T value, TOther otherValue)
        {
            return Option.None<T, TOther>(otherValue);
        }

        /// <summary>
        /// Creates an Option&lt;T&gt; instance from a specified value. 
        /// If the value is null, the returned Option&lt;T&gt; instance is empty.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The Option&lt;T&gt; instance.</returns>
        public static Option<T> SomeNotNull<T>(this T value)
        {
            if (value != null)
            {
                return Option.Some(value);
            }

            return Option.None<T>();
        }

        /// <summary>
        /// Creates an Option&lt;T&gt; instance from a specified value. 
        /// If the value is null, the returned Option&lt;T&gt; instance is empty.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The Option&lt;T&gt; instance.</returns>
        public static Option<T, TException> SomeNotNull<T, TException>(this T value, TException exception)
        {
            if (value != null)
            {
                return Option.Some<T, TException>(value);
            }

            return Option.None<T, TException>(exception);
        }

        /// <summary>
        /// Converts a Nullable&lt;T&gt; to an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="value">The Nullable&lt;T&gt; instance.</param>
        /// <returns>The Option&lt;T&gt; instance.</returns>
        public static Option<T> ToOption<T>(this Nullable<T> value) where T : struct
        {
            if (value.HasValue)
            {
                return Option.Some(value.Value);
            }

            return Option.None<T>();
        }

        /// <summary>
        /// Converts a Nullable&lt;T&gt; to an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="value">The Nullable&lt;T&gt; instance.</param>
        /// <returns>The Option&lt;T&gt; instance.</returns>
        public static Option<T, TException> ToOption<T, TException>(this Nullable<T> value, TException exception) where T : struct
        {
            if (value.HasValue)
            {
                return Option.Some<T, TException>(value.Value);
            }

            return Option.None<T, TException>(exception);
        }
    }
}
