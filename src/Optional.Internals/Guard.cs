using System;
using System.Diagnostics;

namespace Optional.Internals
{
    /// <summary>
    ///  Provides a set of functions for guarding arguments.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        ///  Ensures passed argument is not null reference.
        /// </summary>
        /// <param name="argument"> Argument to validate against. </param>
        /// <exception cref="ArgumentNullException" />
        [DebuggerStepThrough]
        public static void ArgumentNotNull(object argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(nameof(argument));
            }
        }

        /// <summary>
        ///  Ensures that none of passed arguments is null reference.
        /// </summary>
        /// <param name="arguments"> Arguments to validate against. </param>
        /// <exception cref="ArgumentNullException" />
        [DebuggerStepThrough]
        public static void ArgumentsNotNull(params object[] arguments)
        {
            foreach (var argument in arguments)
            {
                ArgumentNotNull(argument);
            }
        }
    }
}