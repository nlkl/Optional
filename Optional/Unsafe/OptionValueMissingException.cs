using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optional.Unsafe
{
    /// <summary>
    /// Indicates a failed retrieval of a value from an empty Option&lt;T&gt; instance.
    /// </summary>
    public class OptionValueMissingException : Exception
    {
        internal OptionValueMissingException()
            : base()
        {
        }

        internal OptionValueMissingException(string message)
            : base(message)
        {
        }
    }
}
