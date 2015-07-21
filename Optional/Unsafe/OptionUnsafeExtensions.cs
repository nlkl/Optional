using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optional.Unsafe
{
    public static class OptionUnsafeExtensions
    {
        /// <summary>
        /// Returns the existing value if present, or throws an OptionValueMissingException.
        /// </summary>
        /// <param name="option">The specified optional.</param>
        /// <returns>The existing value.</returns>
        /// <exception cref="Optional.Unsafe.OptionValueMissingException">Thrown when a value is not present.</exception>
        public static T ValueOrFailure<T>(this Option<T> option)
        {
            if (option.HasValue)
            {
                return option.Value;
            }

            throw new OptionValueMissingException();
        }

        /// <summary>
        /// Returns the existing value if present, or throws an OptionValueMissingException.
        /// </summary>
        /// <param name="option">The specified optional.</param>
        /// <returns>The existing value.</returns>
        /// <exception cref="Optional.Unsafe.OptionValueMissingException">Thrown when a value is not present.</exception>
        public static T ValueOrFailure<T, TException>(this Option<T, TException> option)
        {
            if (option.HasValue)
            {
                return option.Value;
            }

            throw new OptionValueMissingException();
        }

        /// <summary>
        /// Returns the existing value if present, or throws an OptionValueMissingException.
        /// </summary>
        /// <param name="option">The specified optional.</param>
        /// <param name="errorMessage">An error message to use in case of failure.</param>
        /// <returns>The existing value.</returns>
        /// <exception cref="Optional.Unsafe.OptionValueMissingException">Thrown when a value is not present.</exception>
        public static T ValueOrFailure<T>(this Option<T> option, string errorMessage)
        {
            if (option.HasValue)
            {
                return option.Value;
            }

            throw new OptionValueMissingException(errorMessage);
        }

        /// <summary>
        /// Returns the existing value if present, or throws an OptionValueMissingException.
        /// </summary>
        /// <param name="option">The specified optional.</param>
        /// <param name="errorMessageFactory">A factory function generating an error message to use in case of failure.</param>
        /// <returns>The existing value.</returns>
        /// <exception cref="Optional.Unsafe.OptionValueMissingException">Thrown when a value is not present.</exception>
        public static T ValueOrFailure<T>(this Option<T> option, Func<string> errorMessageFactory)
        {
            return option.Match(
                some: value => value, 
                none: () => { throw new OptionValueMissingException(errorMessageFactory()); }
            );
        }

        /// <summary>
        /// Returns the existing value if present, or throws an OptionValueMissingException.
        /// </summary>
        /// <param name="option">The specified optional.</param>
        /// <param name="errorMessage">An error message to use in case of failure.</param>
        /// <returns>The existing value.</returns>
        /// <exception cref="Optional.Unsafe.OptionValueMissingException">Thrown when a value is not present.</exception>
        public static T ValueOrFailure<T, TException>(this Option<T, TException> option, string errorMessage)
        {
            if (option.HasValue)
            {
                return option.Value;
            }

            throw new OptionValueMissingException(errorMessage);
        }

        /// <summary>
        /// Returns the existing value if present, or throws an OptionValueMissingException.
        /// </summary>
        /// <param name="option">The specified optional.</param>
        /// <param name="errorMessageFactory">A factory function generating an error message to use in case of failure.</param>
        /// <returns>The existing value.</returns>
        /// <exception cref="Optional.Unsafe.OptionValueMissingException">Thrown when a value is not present.</exception>
        public static T ValueOrFailure<T, TException>(this Option<T, TException> option, Func<TException, string> errorMessageFactory)
        {
            return option.Match(
                some: value => value, 
                none: exception => { throw new OptionValueMissingException(errorMessageFactory(exception)); }
            );
        }
    }
}
