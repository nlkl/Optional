using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional
{
    public static class OptionExtensions
    {
        /// <summary>
        /// Returns the wrapped value if present, and otherwise an alternative value.
        /// </summary>
        /// <param name="option">The Option&lt;T&gt; instance.</param>
        /// <param name="alternative">The alternative value.</param>
        /// <returns>The wrapped or alternative value.</returns>
        public static T ValueOr<T>(this Option<T> option, T alternative)
        {
            if (option.HasValue)
            {
                return option.Value;
            }

            return alternative;
        }

        /// <summary>
        /// Evaluates a specified function, based on whether a value is present or not.
        /// </summary>
        /// <param name="option">The Option&lt;T&gt; instance.</param>
        /// <param name="some">The function to evaluate if the value is present.</param>
        /// <param name="none">The function to evaluate if the value is missing.</param>
        /// <returns>The result of the evaluated function.</returns>
        public static TResult Match<T, TResult>(this Option<T> option, Func<T, TResult> some, Func<TResult> none)
        {
            if (option.HasValue)
            {
                return some(option.Value);
            }

            return none();
        }

        /// <summary>
        /// Evaluates a specified action, based on whether a value is present or not.
        /// </summary>
        /// <param name="option">The Option&lt;T&gt; instance.</param>
        /// <param name="some">The action to evaluate if the value is present.</param>
        /// <param name="none">The action to evaluate if the value is missing.</param>
        public static void Match<T>(this Option<T> option, Action<T> some, Action none)
        {
            if (option.HasValue)
            {
                some(option.Value);
            }

            none();
        }

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
                return value.Some();
            }

            return Option.None<T>();
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
                return value.Value.Some();
            }

            return Option.None<T>();
        }

        /// <summary>
        /// Transforms the inner value in an Option&lt;T&gt; instance.
        /// If the instance is empty, an empty instance is returned.
        /// </summary>
        /// <param name="option">The Option&lt;T&gt; instance.</param>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed Option&lt;T&gt; instance.</returns>
        public static Option<TResult> Map<T, TResult>(this Option<T> option, Func<T, TResult> mapping)
        {
            return option.Match(
                some: value => mapping(value).Some(),
                none: () => Option.None<TResult>()
            );
        }

        /// <summary>
        /// Transforms the inner value in an Option&lt;T&gt; instance
        /// into another Option&lt;T&gt; instance. The result is flattened, 
        /// and if either is empty, an empty instance is returned.
        /// </summary>
        /// <param name="option">The Option&lt;T&gt; instance.</param>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed Option&lt;T&gt; instance.</returns>
        public static Option<TResult> FlatMap<T, TResult>(this Option<T> option, Func<T, Option<TResult>> mapping)
        {
            return option.Match(
                some: value => mapping(value),
                none: () => Option.None<TResult>()
            );
        }

        /// <summary>
        /// Empties an Option&lt;T&gt; instance, if a specified predicate
        /// is not satisfied.
        /// </summary>
        /// <param name="option">The Option&lt;T&gt; instance.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The filtered Option&lt;T&gt; instance.</returns>
        public static Option<T> Filter<T>(this Option<T> option, Func<T, bool> predicate)
        {
            return option.Match(
                some: value => predicate(value) ? value.Some() : Option.None<T>(),
                none: () => Option.None<T>()
            );
        }
    }
}
