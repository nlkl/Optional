﻿using System;
using System.Threading.Tasks;

namespace Optional.Async.Tests
{
    public static class ValueGenerator
    {
        public static async Task<T> DelayedValue<T>(T value)
        {
            await Task.Delay(100).ConfigureAwait(false);
            return value;
        }

        public static string RandomString() => Guid.NewGuid().ToString();
        public static Func<T, Task<TResult>> AsyncOperation<T, TResult>(TResult result) => _ => Task.FromResult(result);
        public static Func<T, TResult> SynchronousOperation<T, TResult>(TResult result) => _ => result;

        public static Task<Option<T>> DelayedSome<T>(T value) => DelayedValue(value.Some());
        public static Task<Option<T, TException>> DelayedSome<T, TException>(T value) => DelayedValue(value.Some<T, TException>());

        public static Task<Option<T>> DelayedNone<T>() => DelayedValue(Option.None<T>());
        public static Task<Option<T, TException>> DelayedNone<T, TException>(TException exception) => DelayedValue(Option.None<T, TException>(exception));
    }
}
