using System;

namespace Optional.Unsafe
{
    /// <summary>
    /// Indicates a failed retrieval of a value from an empty optional.
    /// </summary>
    public sealed class OptionValueMissingException : Exception
    {
        public OptionValueMissingException()
        {
        }

        public OptionValueMissingException(string message)
            : base(message)
        {
        }
    }
}
