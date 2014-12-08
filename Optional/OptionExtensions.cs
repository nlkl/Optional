using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional
{
    public static class OptionExtensions
    {
        public static T ValueOr<T>(this Option<T> option, T alternative)
        {
            if (option.HasValue)
            {
                return option.Value;
            }

            return alternative;
        }

        public static TResult Match<T, TResult>(this Option<T> option, Func<T, TResult> some, Func<TResult> none)
        {
            if (option.HasValue)
            {
                return some(option.Value);
            }

            return none();
        }

        public static void Match<T>(this Option<T> option, Action<T> some, Action none)
        {
            if (option.HasValue)
            {
                some(option.Value);
            }

            none();
        }

        public static Option<T> Some<T>(this T value)
        {
            return Option.Some(value);
        }

        public static Option<T> None<T>(this T value)
        {
            return Option.None<T>();
        }

        public static Option<T> SomeNotNull<T>(this T value)
        {
            if (value != null)
            {
                return value.Some();
            }

            return Option.None<T>();
        }

        public static Option<T> ToOption<T>(this Nullable<T> value) where T : struct
        {
            if (value.HasValue)
            {
                return value.Value.Some();
            }

            return Option.None<T>();
        }

        public static Option<TResult> Map<T, TResult>(this Option<T> option, Func<T, TResult> mapping)
        {
            return option.Match(
                some: value => mapping(value).Some(),
                none: () => Option.None<TResult>()
            );
        }

        public static Option<TResult> FlatMap<T, TResult>(this Option<T> option, Func<T, Option<TResult>> mapping)
        {
            return option.Match(
                some: value => mapping(value),
                none: () => Option.None<TResult>()
            );
        }

        public static Option<T> Filter<T>(this Option<T> option, Func<T, bool> predicate)
        {
            return option.Match(
                some: value => predicate(value) ? value.Some() : Option.None<T>(),
                none: () => Option.None<T>()
            );
        }
    }
}
