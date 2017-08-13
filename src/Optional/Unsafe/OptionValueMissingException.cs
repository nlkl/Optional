using System;

namespace Optional.Unsafe
{
    /// <summary>
    /// Indicates a failed retrieval of a value from an empty optional.
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
