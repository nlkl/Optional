using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional
{
    public struct Option<T>
    {
        private bool hasValue;
        private T value;

        public bool HasValue { get { return hasValue; } }
        internal T Value { get { return value; } }

        internal Option(T value, bool hasValue)
        {
            this.value = value;
            this.hasValue = hasValue;
        }
    }

    public static class Option
    {
        public static Option<T> Some<T>(T value)
        {
            return new Option<T>(value, true);
        }

        public static Option<T> None<T>()
        {
            return new Option<T>(default(T), false);
        }
    }
}
