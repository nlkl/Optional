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

        /// <summary>
        /// Determines whether two optionals are equal.
        /// </summary>
        /// <param name="obj">The optional to compare with the current one.</param>
        /// <returns>A boolean indicating whether or not the optionals are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is Option<T>)
            {
                var other = (Option<T>)obj;

                if (!hasValue && !other.hasValue)
                {
                    return true;
                }
                else if (hasValue && other.hasValue)
                {
                    if (value == null && other.value == null)
                    {
                        return true;
                    }
                    else if (value != null && other.value != null)
                    {
                        return value.Equals(other.value);
                    }
                }
            }

            return false;
        }

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
        /// Returns the existing value if present, and otherwise an alternative value.
        /// </summary>
        /// <param name="alternative">The alternative value.</param>
        /// <returns>The existing or alternative value.</returns>
        public T ValueOr(T alternative)
        {
            if (HasValue)
            {
                return value;
            }

            return alternative;
        }

        /// <summary>
        /// Uses an alternative value, if no existing value is present.
        /// </summary>
        /// <param name="alternative">The alternative value.</param>
        /// <returns>A new optional, containing either the existing or alternative value.</returns>
        public Option<T> Or(T alternative)
        {
            if (HasValue)
            {
                return this;
            }

            return Option.Some(alternative);
        }

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
        /// Evaluates a specified function, based on whether a value is present or not.
        /// </summary>
        /// <param name="some">The function to evaluate if the value is present.</param>
        /// <param name="none">The function to evaluate if the value is missing.</param>
        /// <returns>The result of the evaluated function.</returns>
        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            if (HasValue)
            {
                return some(value);
            }

            return none();
        }

        /// <summary>
        /// Evaluates a specified action, based on whether a value is present or not.
        /// </summary>
        /// <param name="some">The action to evaluate if the value is present.</param>
        /// <param name="none">The action to evaluate if the value is missing.</param>
        public void Match(Action<T> some, Action none)
        {
            if (HasValue)
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
        public Option<TResult> FlatMap<TResult, TException>(Func<T, Option<TResult, TException>> mapping)
        {
            return FlatMap(value => mapping(value).WithoutException());
        }

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

    /// <summary>
    /// Represents an optional value, along with a potential exceptional value.
    /// </summary>
    /// <typeparam name="T">The type of the value to be wrapped.</typeparam>
    /// <typeparam name="TException">A exceptional value describing the lack of an actual value.</typeparam>
    public struct Option<T, TException>
    {
        private bool hasValue;
        private T value;
        private TException exception;

        /// <summary>
        /// Checks if a value is present.
        /// </summary>
        public bool HasValue { get { return hasValue; } }

        internal T Value { get { return value; } }
        internal TException Exception { get { return exception; } }

        internal Option(T value, TException exception, bool hasValue)
        {
            this.value = value;
            this.hasValue = hasValue;
            this.exception = exception;
        }

        /// <summary>
        /// Determines whether two optionals are equal.
        /// </summary>
        /// <param name="obj">The optional to compare with the current one.</param>
        /// <returns>A boolean indicating whether or not the optionals are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is Option<T, TException>)
            {
                var other = (Option<T, TException>)obj;

                if (!hasValue && !other.hasValue)
                {
                    if (exception == null && other.exception == null)
                    {
                        return true;
                    }
                    else if (exception != null && other.exception != null)
                    {
                        return exception.Equals(other.exception);
                    }
                }
                else if (hasValue && other.hasValue)
                {
                    if (value == null && other.value == null)
                    {
                        return true;
                    }
                    else if (value != null && other.value != null)
                    {
                        return value.Equals(other.value);
                    }
                }
            }

            return false;
        }

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

            if (exception == null)
            {
                return 0;
            }

            return exception.GetHashCode();
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

            if (exception == null)
            {
                return "None(null)";
            }

            return string.Format("None({0})", exception);
        }

        /// <summary>
        /// Returns the existing value if present, and otherwise an alternative value.
        /// </summary>
        /// <param name="alternative">The alternative value.</param>
        /// <returns>The existing or alternative value.</returns>
        public T ValueOr(T alternative)
        {
            if (HasValue)
            {
                return value;
            }

            return alternative;
        }

        /// <summary>
        /// Uses an alternative value, if no existing value is present.
        /// </summary>
        /// <param name="alternative">The alternative value.</param>
        /// <returns>A new optional, containing either the existing or alternative value.</returns>
        public Option<T, TException> Or(T alternative)
        {
            if (HasValue)
            {
                return this;
            }

            return Option.Some<T, TException>(alternative);
        }

        /// <summary>
        /// Forgets any attached exceptional value.
        /// </summary>
        /// <returns>An optional without an exceptional value.</returns>
        public Option<T> WithoutException()
        {
            return Match(
                some: value => Option.Some(value),
                none: _ => Option.None<T>()
            );
        }

        /// <summary>
        /// Evaluates a specified function, based on whether a value is present or not.
        /// </summary>
        /// <param name="some">The function to evaluate if the value is present.</param>
        /// <param name="none">The function to evaluate if the value is missing.</param>
        /// <returns>The result of the evaluated function.</returns>
        public TResult Match<TResult>(Func<T, TResult> some, Func<TException, TResult> none)
        {
            if (HasValue)
            {
                return some(value);
            }

            return none(exception);
        }

        /// <summary>
        /// Evaluates a specified action, based on whether a value is present or not.
        /// </summary>
        /// <param name="some">The action to evaluate if the value is present.</param>
        /// <param name="none">The action to evaluate if the value is missing.</param>
        public void Match(Action<T> some, Action<TException> none)
        {
            if (HasValue)
            {
                some(value);
            }
            else
            {
                none(exception);
            }
        }

        /// <summary>
        /// Transforms the inner value in an optional.
        /// If the instance is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public Option<TResult, TException> Map<TResult>(Func<T, TResult> mapping)
        {
            return Match(
                some: value => Option.Some<TResult, TException>(mapping(value)),
                none: exception => Option.None<TResult, TException>(exception)
            );
        }

        /// <summary>
        /// Transforms the exceptional value in an optional.
        /// If the instance is not empty, no transformation is carried out.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public Option<T, TExceptionResult> MapException<TExceptionResult>(Func<TException, TExceptionResult> mapping)
        {
            return Match(
                some: value => Option.Some<T, TExceptionResult>(value),
                none: exception => Option.None<T, TExceptionResult>(mapping(exception))
            );
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <returns>The transformed optional.</returns>
        public Option<TResult, TException> FlatMap<TResult>(Func<T, Option<TResult, TException>> mapping)
        {
            return Match(
                some: value => mapping(value),
                none: exception => Option.None<TResult, TException>(exception)
            );
        }

        /// <summary>
        /// Transforms the inner value in an optional
        /// into another optional. The result is flattened, 
        /// and if either is empty, an empty optional is returned, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="mapping">The transformation function.</param>
        /// <param name="exception">The exceptional value to attach.</param>
        /// <returns>The transformed optional.</returns>
        public Option<TResult, TException> FlatMap<TResult>(Func<T, Option<TResult>> mapping, TException exception)
        {
            return FlatMap(value => mapping(value).WithException(exception));
        }

        /// <summary>
        /// Empties an optional, and attaches an exceptional value, 
        /// if a specified predicate is not satisfied.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="exception">The exceptional value to attach.</param>
        /// <returns>The filtered optional.</returns>
        public Option<T, TException> Filter(Func<T, bool> predicate, TException exception)
        {
            var original = this;
            return Match(
                some: value => predicate(value) ? original : Option.None<T, TException>(exception),
                none: _ => original
            );
        }
    }

    /// <summary>
    /// Provides a set of functions for creating optional values.
    /// </summary>
    public static class Option
    {
        /// <summary>
        /// Wraps an existing value in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T> Some<T>(T value)
        {
            return new Option<T>(value, true);
        }

        /// <summary>
        /// Wraps an existing value in an Option&lt;T, TException&gt; instance.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T, TException> Some<T, TException>(T value)
        {
            return new Option<T, TException>(value, default(TException), true);
        }

        /// <summary>
        /// Creates an empty Option&lt;T&gt; instance.
        /// </summary>
        /// <returns>An empty optional.</returns>
        public static Option<T> None<T>()
        {
            return new Option<T>(default(T), false);
        }

        /// <summary>
        /// Creates an empty Option&lt;T, TException&gt; instance, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="exception">The exceptional value.</param>
        /// <returns>An empty optional.</returns>
        public static Option<T, TException> None<T, TException>(TException exception)
        {
            return new Option<T, TException>(default(T), exception, false);
        }
    }
}
