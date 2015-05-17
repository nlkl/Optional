using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions
{
    public static class Try
    {
        public static Option<T, TException> Run<T, TException>(Func<T> operation)
            where TException : Exception
        {
            try
            {
                return Option.Some<T, TException>(operation());
            }
            catch (TException ex)
            {
                return Option.None<T, TException>(ex);
            }
        }

        public static Option<T, Exception> Run<T>(Func<T> operation)
        {
            try
            {
                return Option.Some<T, Exception>(operation());
            }
            catch (Exception ex)
            {
                return Option.None<T, Exception>(ex);
            }
        }
    }
}
