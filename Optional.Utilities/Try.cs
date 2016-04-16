using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Utilities
{
    public static class Try
    {
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

        public static Option<T, Exception> Run<T, TException>(Func<T> operation)
            where TException : Exception
        {
            try
            {
                return Option.Some<T, Exception>(operation());
            }
            catch (TException ex)
            {
                return Option.None<T, Exception>(ex);
            }
        }

        public static Option<T, Exception> Run<T, TException1, TException2>(Func<T> operation)
            where TException1 : Exception
            where TException2 : Exception
        {
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

        public static Option<T, Exception> Run<T, TException1, TException2, TException3>(Func<T> operation)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
        {
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

        public static Option<T, Exception> Run<T, TException1, TException2, TException3, TException4>(Func<T> operation)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TException4 : Exception
        {
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

        public static Option<T, Exception> Run<T, TException1, TException2, TException3, TException4, TException5>(Func<T> operation)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TException4 : Exception
            where TException5 : Exception
        {
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
