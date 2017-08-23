using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optional.Utilities
{
    /// <summary>
    /// Utility functionality for catching and wrapping exceptions in an optional.
    /// </summary>
    public static class Safe
    {
        /// <summary>
        /// Executes an operation safely, catching any potential
        /// exceptions and wrapping the result in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="operation">The operation to perform.</param>
        /// <returns>An Option&lt;T&gt; instance containing the result or a caught exception.</returns>
        public static Option<T, Exception> Try<T>(Func<T> operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            try
            {
                return Option.Some<T, Exception>(operation());
            }
            catch (Exception ex)
            {
                return Option.None<T, Exception>(ex);
            }
        }

        /// <summary>
        /// Executes an operation safely, catching any potential
        /// exceptions of the specified type and wrapping the result
        /// in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="operation">The operation to perform.</param>
        /// <returns>An Option&lt;T&gt; instance containing the result or a caught exception.</returns>
        public static Option<T, Exception> Try<T, TException>(Func<T> operation)
            where TException : Exception
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            try
            {
                return Option.Some<T, Exception>(operation());
            }
            catch (TException ex)
            {
                return Option.None<T, Exception>(ex);
            }
        }

        /// <summary>
        /// Executes an operation safely, catching any potential
        /// exceptions of one of the specified types and wrapping
        /// the result in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="operation">The operation to perform.</param>
        /// <returns>An Option&lt;T&gt; instance containing the result or a caught exception.</returns>
        public static Option<T, Exception> Try<T, TException1, TException2>(Func<T> operation)
            where TException1 : Exception
            where TException2 : Exception
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            try
            {
                return Option.Some<T, Exception>(operation());
            }
            catch (TException1 ex)
            {
                return Option.None<T, Exception>(ex);
            }
            catch (TException2 ex)
            {
                return Option.None<T, Exception>(ex);
            }
        }

        /// <summary>
        /// Executes an operation safely, catching any potential
        /// exceptions of one of the specified types and wrapping
        /// the result in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="operation">The operation to perform.</param>
        /// <returns>An Option&lt;T&gt; instance containing the result or a caught exception.</returns>
        public static Option<T, Exception> Try<T, TException1, TException2, TException3>(Func<T> operation)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            try
            {
                return Option.Some<T, Exception>(operation());
            }
            catch (TException1 ex)
            {
                return Option.None<T, Exception>(ex);
            }
            catch (TException2 ex)
            {
                return Option.None<T, Exception>(ex);
            }
            catch (TException3 ex)
            {
                return Option.None<T, Exception>(ex);
            }
        }

        /// <summary>
        /// Executes an operation safely, catching any potential
        /// exceptions of one of the specified types and wrapping
        /// the result in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="operation">The operation to perform.</param>
        /// <returns>An Option&lt;T&gt; instance containing the result or a caught exception.</returns>
        public static Option<T, Exception> Try<T, TException1, TException2, TException3, TException4>(Func<T> operation)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TException4 : Exception
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            try
            {
                return Option.Some<T, Exception>(operation());
            }
            catch (TException1 ex)
            {
                return Option.None<T, Exception>(ex);
            }
            catch (TException2 ex)
            {
                return Option.None<T, Exception>(ex);
            }
            catch (TException3 ex)
            {
                return Option.None<T, Exception>(ex);
            }
            catch (TException4 ex)
            {
                return Option.None<T, Exception>(ex);
            }
        }

        /// <summary>
        /// Executes an operation safely, catching any potential
        /// exceptions of one of the specified types and wrapping
        /// the result in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="operation">The operation to perform.</param>
        /// <returns>An Option&lt;T&gt; instance containing the result or a caught exception.</returns>
        public static Option<T, Exception> Try<T, TException1, TException2, TException3, TException4, TException5>(Func<T> operation)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TException4 : Exception
            where TException5 : Exception
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            try
            {
                return Option.Some<T, Exception>(operation());
            }
            catch (TException1 ex)
            {
                return Option.None<T, Exception>(ex);
            }
            catch (TException2 ex)
            {
                return Option.None<T, Exception>(ex);
            }
            catch (TException3 ex)
            {
                return Option.None<T, Exception>(ex);
            }
            catch (TException4 ex)
            {
                return Option.None<T, Exception>(ex);
            }
            catch (TException5 ex)
            {
                return Option.None<T, Exception>(ex);
            }
        }
    }
}
