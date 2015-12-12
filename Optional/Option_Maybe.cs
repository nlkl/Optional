using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optional
{
    /// <summary>
    /// Represents an optional value.
    /// </summary>
    /// <typeparam name="T">The type of the value to be wrapped.</typeparam>
    public struct Option<T> : IEquatable<Option<T>>
    {
        private readonly bool hasValue;
        private readonly T value;

        /// <summary>
        /// Checks if a value is present.
        /// </summary>
        public bool HasValue => hasValue;

        internal T Value => value;

        internal Option(T value, bool hasValue)
        {
            this.value = value;
            this.hasValue = hasValue;
        }

        /// <summary>
        /// Determines whether two optionals are equal.
        /// </summary>
        /// <param name="other">The optional to compare with the current one.</param>
        /// <returns>A boolean indicating whether or not the optionals are equal.</returns>
        public bool Equals(Option<T> other)
        {
            if (!hasValue && !other.hasValue)
            {
                return true;
            }
            else if (hasValue && other.hasValue)
            {
                return EqualityComparer<T>.Default.Equals(value, other.value);
            }

            return false;
        }

        /// <summary>
        /// Determines whether two optionals are equal.
        /// </summary>
        /// <param name="obj">The optional to compare with the current one.</param>
        /// <returns>A boolean indicating whether or not the optionals are equal.</returns>
        public override bool Equals(object obj) => obj is Option<T> ? Equals((Option<T>)obj) : false;

        /// <summary>
        /// Determines whether two optionals are equal.
        /// </summary>
        /// <param name="left">The first optional to compare.</param>
        /// <param name="right">The second optional to compare.</param>
        /// <returns>A boolean indicating whether or not the optionals are equal.</returns>
        public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);

        /// <summary>
        /// Determines whether two optionals are unequal.
        /// </summary>
        /// <param name="left">The first optional to compare.</param>
        /// <param name="right">The second optional to compare.</param>
        /// <returns>A boolean indicating whether or not the optionals are unequal.</returns>
        public static bool operator !=(Option<T> left, Option<T> right) => !left.Equals(right);

        /// <summary>
        /// Generates a hash code for the current optional.
        /// </summary>
        /// <returns>A hash code for the current optional.</returns>
        public override int GetHashCode()
        {
            if (hasValue)
            {
                if (value == null)
                {
                    return 1;
                }

                return value.GetHashCode();
            }

            return 0;
        }

        /// <summary>
        /// Returns a string that represents the current optional.
        /// </summary>
        /// <returns>A string that represents the current optional.</returns>
        public override string ToString()
        {
            if (hasValue)
            {
                if (value == null)
                {
                    return "Some(null)";
                }

                return string.Format("Some({0})", value);
            }

            return "None";
        }

        /// <summary>
        /// Converts the current optional into an enumerable with one or zero elements.
        /// </summary>
        /// <returns>A corresponding enumerable.</returns>
        public IEnumerable<T> ToEnumerable()
        {
            if (hasValue)
            {
                yield return value;
            }
        }

        /// <summary>
        /// Returns an enumerator for the optional.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (hasValue)
            {
                yield return value;
            }
        }

        /// <summary>
        /// Determines if the current optional contains a specified value.
        /// </summary>
        /// <param name="value">The value to locate.</param>
        /// <returns>A boolean indicating whether or not the value was found.</returns>
        public bool Contains(T value)
        {
            if (hasValue)
            {
                if (this.value == null)
                {
                    return value == null;
                }

                return this.value.Equals(value);
            }

            return false;
        }

        /// <summary>
        /// Determines if the current optional contains a value 
        /// satisfying a specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A boolean indicating whether or not the predicate was satisfied.</returns>
        public bool Exists(Func<T, bool> predicate) => hasValue && predicate(value);

        /// <summary>
        /// Returns the existing value if present, and otherwise an alternative value.
        /// </summary>
        /// <param name="alternative">The alternative value.</param>
        /// <returns>The existing or alternative value.</returns>
        public T ValueOr(T alternative) => hasValue ? value : alternative;

        /// <summary>
        /// Returns the existing value if present, and otherwise an alternative value.
        /// </summary>
        /// <param name="alternativeFactory">A factory function to create an alternative value.</param>
        /// <returns>The existing or alternative value.</returns>
        public T ValueOr(Func<T> alternativeFactory) => hasValue ? value : alternativeFactory();

        /// <summary>
        /// Uses an alternative value, if no existing value is present.
        /// </summary>
        /// <param name="alternative">The alternative value.</param>
        /// <returns>A new optional, containing either the existing or alternative value.</returns>
        public Option<T> Or(T alternative) => hasValue ? this : Option.Some(alternative);

        /// <summary>
        /// Uses an alternative value, if no existing value is present.
        /// </summary>
        /// <param name="alternativeFactory">A factory function to create an alternative value.</param>
        /// <returns>A new optional, containing either the existing or alternative value.</returns>
        public Option<T> Or(Func<T> alternativeFactory) => hasValue ? this : Option.Some(alternativeFactory());

        /// <summary>
        /// Attaches an exceptional value to an empty optional.
        /// </summary>
        /// <param name="exception">The exceptional value to attach.</param>
        /// <returns>An optional with an exceptional value.</returns>
        public Option<T, TException> WithException<TException>(TException exception)
        {
            return Match(
                some: value => Option.Some<T, TException>(value),
                none: () => Option.None<T, TException>(exception)
            );
        }

        /// <summary>
        /// Attaches an exceptional value to an empty optional.
        /// </summary>
        /// <param name="exceptionFactory">A factory function to create an exceptional value to attach.</param>
        /// <returns>An optional with an exceptional value.</returns>
        public Option<T, TException> WithException<TException>(Func<TException> exceptionFactory)
        {
            return Match(
                some: value => Option.Some<T, TException>(value),
                none: () => Option.None<T, TException>(exceptionFactory())
            );
        }

        /// <summary>
        /// Evaluates a specified function, based on whether a value is present or not.
        /// </summary>
        /// <param name="some">The function to evaluate if the value is present.</param>
        /// <param name="none">The function to evaluate if the value is missing.</param>
        /// <returns>The result of the evaluated function.</returns>
        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none) => hasValue ? some(value) : none();

        /// <summary>
        /// Evaluates a specified action, based on whether a value is present or not.
        /// </summary>
        /// <param name="some">The action to evaluate if the value is present.</param>
        /// <param name="none">The action to evaluate if the value is missing.</param>
        public void Match(Action<T> some, Action none)
        {
            if (hasValue)
            {
                some(value);
            }
            else
            {
                none();
            }
        }

        /// <summary>
        /// Transforms the inner value in an optional.
        /// If the instance is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public Option<TResult> Map<TResult>(Func<T, TResult> mapping)
        {
            return Match(
                some: value => Option.Some(mapping(value)),
                none: () => Option.None<TResult>()
            );
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public Option<TResult> FlatMap<TResult>(Func<T, Option<TResult>> mapping)
        {
            return Match(
                some: value => mapping(value),
                none: () => Option.None<TResult>()
            );
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// If the option contains an exception, it is removed.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public Option<TResult> FlatMap<TResult, TException>(Func<T, Option<TResult, TException>> mapping) =>
            FlatMap(value => mapping(value).WithoutException());

        /// <summary>
        /// Empties an optional, if a specified condition
        /// is not satisfied.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The filtered optional.</returns>
        public Option<T> Filter(bool condition) => hasValue && !condition ? Option.None<T>() : this;

        /// <summary>
        /// Empties an optional, if a specified predicate
        /// is not satisfied.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The filtered optional.</returns>
        public Option<T> Filter(Func<T, bool> predicate)
        {
            var original = this;
            return Match(
                some: value => predicate(value) ? original : Option.None<T>(),
                none: () => original
            );
        }
    }
}
