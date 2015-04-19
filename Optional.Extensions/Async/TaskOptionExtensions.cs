using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions.Async
{
    public static class TaskOptionExtensions
    {
        public static Task<Option<TResult>> Map<T, TResult>(this Task<Option<T>> task, Func<T, TResult> mapping)
        {
            return task.Map(option => option.Map(mapping));
        }

        public static Task<Option<TResult>> FlatMap<T, TResult>(this Task<Option<T>> task, Func<T, Task<Option<TResult>>> mapping)
        {
            return task.FlatMap(option => option
                .Match(
                    some: value => mapping(value), 
                    none: () => Task.FromResult(Option.None<TResult>())
                ));
        }

        public static Task<Option<TResult>> TaskMap<T, TResult>(this Task<Option<T>> task, Func<T, Task<TResult>> mapping)
        {
            return task.FlatMap(value => mapping(value).Map(result => result.Some()));
        }

        public static Task<Option<TResult>> OptionMap<T, TResult>(this Task<Option<T>> task, Func<T, Option<TResult>> mapping)
        {
            return task.FlatMap(value => Task.FromResult(mapping(value)));
        }

        public static Task<Option<TResult, TException>> Map<T, TResult, TException>(this Task<Option<T, TException>> task, Func<T, TResult> mapping)
        {
            return task.Map(option => option.Map(mapping));
        }

        public static Task<Option<TResult, TException>> FlatMap<T, TResult, TException>(this Task<Option<T, TException>> task, Func<T, Task<Option<TResult, TException>>> mapping)
        {
            return task.FlatMap(option => option
                .Match(
                    some: value => mapping(value),
                    none: exception => Task.FromResult(Option.None<TResult, TException>(exception))
                ));
        }

        public static Task<Option<TResult, TException>> TaskMap<T, TResult, TException>(this Task<Option<T, TException>> task, Func<T, Task<TResult>> mapping)
        {
            return task.FlatMap(value => mapping(value).Map(result => result.Some<TResult, TException>()));
        }

        public static Task<Option<TResult, TException>> OptionMap<T, TResult, TException>(this Task<Option<T, TException>> task, Func<T, Option<TResult, TException>> mapping)
        {
            return task.FlatMap(value => Task.FromResult(mapping(value)));
        }
    }
}
