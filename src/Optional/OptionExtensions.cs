using System;

namespace Optional
{
    public static class OptionExtensions
    {
        /// <summary>
        /// Wraps an existing value in an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T> Some<T>(this T value) => Option.Some(value);

        /// <summary>
        /// Wraps an existing value in an Option&lt;T, TException&gt; instance.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T, TException> Some<T, TException>(this T value) =>
            Option.Some<T, TException>(value);

        /// <summary>
        /// Creates an empty Option&lt;T&gt; instance from a specified value.
        /// </summary>
        /// <param name="value">A value determining the type of the optional.</param>
        /// <returns>An empty optional.</returns>
        public static Option<T> None<T>(this T value) => Option.None<T>();

        /// <summary>
        /// Creates an empty Option&lt;T, TException&gt; instance, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="exception">The exceptional value.</param>
        /// <returns>An empty optional.</returns>
        public static Option<T, TException> None<T, TException>(this T value, TException exception) =>
            Option.None<T, TException>(exception);

        /// <summary>
        /// Creates an Option&lt;T&gt; instance from a specified value. 
        /// If the value does not satisfy the given predicate, 
        /// an empty optional is returned.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T> SomeWhen<T>(this T value, Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return predicate(value) ? Option.Some(value) : Option.None<T>();
        }

        /// <summary>
        /// Creates an Option&lt;T&gt; instance from a specified value. 
        /// If the value does not satisfy the given predicate, 
        /// an empty optional is returned, with a specified exceptional value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="exception">The exceptional value.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T, TException> SomeWhen<T, TException>(this T value, Func<T, bool> predicate, TException exception)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return predicate(value) ? Option.Some<T, TException>(value) : Option.None<T, TException>(exception);
        }

        /// <summary>
        /// Creates an Option&lt;T&gt; instance from a specified value. 
        /// If the value does not satisfy the given predicate, 
        /// an empty optional is returned, with a specified exceptional value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="exceptionFactory">A factory function to create an exceptional value.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T, TException> SomeWhen<T, TException>(this T value, Func<T, bool> predicate, Func<TException> exceptionFactory)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));
            return predicate(value) ? Option.Some<T, TException>(value) : Option.None<T, TException>(exceptionFactory());
        }

        /// <summary>
        /// Creates an Option&lt;T&gt; instance from a specified value. 
        /// If the value satisfies the given predicate, 
        /// an empty optional is returned.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T> NoneWhen<T>(this T value, Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return value.SomeWhen(val => !predicate(val));
        }

        /// <summary>
        /// Creates an Option&lt;T&gt; instance from a specified value. 
        /// If the value satisfies the given predicate, 
        /// an empty optional is returned, with a specified exceptional value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="exception">The exceptional value.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T, TException> NoneWhen<T, TException>(this T value, Func<T, bool> predicate, TException exception)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return value.SomeWhen(val => !predicate(val), exception);
        }

        /// <summary>
        /// Creates an Option&lt;T&gt; instance from a specified value. 
        /// If the value does satisfy the given predicate, 
        /// an empty optional is returned, with a specified exceptional value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="exceptionFactory">A factory function to create an exceptional value.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T, TException> NoneWhen<T, TException>(this T value, Func<T, bool> predicate, Func<TException> exceptionFactory)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));
            return value.SomeWhen(val => !predicate(val), exceptionFactory);
        }

        /// <summary>
        /// Creates an Option&lt;T&gt; instance from a specified value. 
        /// If the value is null, an empty optional is returned.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T> SomeNotNull<T>(this T? value) => value is null ? Option.None<T>() : Some(value);

        /// <summary>
        /// Creates an Option&lt;T&gt; instance from a specified value. 
        /// If the value is null, an empty optional is returned, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="exception">The exceptional value.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T, TException> SomeNotNull<T, TException>(this T? value, TException exception) =>
            value.SomeNotNull().WithException(exception);

        /// <summary>
        /// Creates an Option&lt;T&gt; instance from a specified value. 
        /// If the value is null, an empty optional is returned, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="exceptionFactory">A factory function to create an exceptional value.</param>
        /// <returns>An optional containing the specified value.</returns>
        public static Option<T, TException> SomeNotNull<T, TException>(this T? value, Func<TException> exceptionFactory) =>
            value.SomeNotNull().WithException(exceptionFactory);

        /// <summary>
        /// Converts a Nullable&lt;T&gt; to an Option&lt;T&gt; instance.
        /// </summary>
        /// <param name="value">The Nullable&lt;T&gt; instance.</param>
        /// <returns>The Option&lt;T&gt; instance.</returns>
        public static Option<T> ToOption<T>(this T? value) where T : struct =>
            value.HasValue ? Option.Some(value.Value) : Option.None<T>();

        /// <summary>
        /// Converts a Nullable&lt;T&gt; to an Option&lt;T, TException&gt; instance, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="value">The Nullable&lt;T&gt; instance.</param>
        /// <param name="exception">The exceptional value.</param>
        /// <returns>The Option&lt;T, TException&gt; instance.</returns>
        public static Option<T, TException> ToOption<T, TException>(this T? value, TException exception) where T : struct =>
            value.HasValue ? Option.Some<T, TException>(value.Value) : Option.None<T, TException>(exception);

        /// <summary>
        /// Converts a Nullable&lt;T&gt; to an Option&lt;T, TException&gt; instance, 
        /// with a specified exceptional value.
        /// </summary>
        /// <param name="value">The Nullable&lt;T&gt; instance.</param>
        /// <param name="exceptionFactory">A factory function to create an exceptional value.</param>
        /// <returns>The Option&lt;T, TException&gt; instance.</returns>
        public static Option<T, TException> ToOption<T, TException>(this T? value, Func<TException> exceptionFactory) where T : struct
        {
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));
            return value.HasValue ? Option.Some<T, TException>(value.Value) : Option.None<T, TException>(exceptionFactory());
        }

        /// <summary>
        /// Returns the existing value if present, or the attached 
        /// exceptional value.
        /// </summary>
        /// <param name="option">The specified optional.</param>
        /// <returns>The existing or exceptional value.</returns>
        public static T ValueOrException<T>(this Option<T, T> option) => option.HasValue ? option.Value : option.Exception;

        /// <summary>
        /// Flattens two nested optionals into one. The resulting optional
        /// will be empty if either the inner or outer optional is empty.
        /// </summary>
        /// <param name="option">The nested optional.</param>
        /// <returns>A flattened optional.</returns>
        public static Option<T> Flatten<T>(this Option<Option<T>> option) =>
            option.FlatMap(innerOption => innerOption);

        /// <summary>
        /// Flattens two nested optionals into one. The resulting optional
        /// will be empty if either the inner or outer optional is empty.
        /// </summary>
        /// <param name="option">The nested optional.</param>
        /// <returns>A flattened optional.</returns>
        public static Option<T, TException> Flatten<T, TException>(this Option<Option<T, TException>, TException> option) =>
            option.FlatMap(innerOption => innerOption);

        /// <summary>
        /// Empties an optional if the value is null.
        /// </summary>
        /// <returns>The filtered optional.</returns>
        public static Option<T> NotNull<T>(this Option<T?> option)
            => option is { HasValue: true, Value: { } value }
                ? Some(value)
                : Option.None<T>();

        /// <summary>
        /// Empties an optional, and attaches an exceptional value, 
        /// if the value is null.
        /// </summary>
        /// <param name="option">The specified optional.</param>
        /// <param name="exception">The exceptional value to attach.</param>
        /// <returns>The filtered optional.</returns>
        public static Option<T, TException> NotNull<T, TException>(this Option<T?, TException> option, TException exception) =>
            option switch
            {
                { HasValue: true, Value: null } => Option.None<T, TException>(exception), 
                { HasValue: true, Value: { } value } =>Some<T, TException>(value),
                _ => Option.None<T, TException>(option.Exception)
            };

        /// <summary>
        /// Empties an optional, and attaches an exceptional value, 
        /// if the value is null.
        /// </summary>
        /// <param name="option">The specified optional.</param>
        /// <param name="exceptionFactory">A factory function to create an exceptional value to attach.</param>
        /// <returns>The filtered optional.</returns>
        public static Option<T, TException> NotNull<T, TException>(this Option<T?, TException> option, Func<TException> exceptionFactory)
        {
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));
            return option switch
            {
                { HasValue: true, Value: null } => Option.None<T, TException>(exceptionFactory()),
                { HasValue: true, Value: { } value } =>Some<T, TException>(value),
                _ => Option.None<T, TException>(option.Exception)
            };
        }

        /// <summary>
        /// Empties an optional if the value is null.
        /// </summary>
        /// <returns>The filtered optional.</returns>
        public static Option<T> NotDefault<T>(this Option<T?> option) where T : struct =>
            option is { HasValue: true, Value: { } value }
                ? Some(value)
                : Option.None<T>();

        /// <summary>
        /// Empties an optional, and attaches an exceptional value, 
        /// if the value is null.
        /// </summary>
        /// <param name="option">The specified optional.</param>
        /// <param name="exception">The exceptional value to attach.</param>
        /// <returns>The filtered optional.</returns>
        public static Option<T, TException> NotDefault<T, TException>(this Option<T?, TException> option, TException exception) 
            where T : struct =>
            option switch
            {
                { HasValue: true, Value: null } => Option.None<T, TException>(exception), 
                { HasValue: true, Value: { } value } =>Some<T, TException>(value),
                _ => Option.None<T, TException>(option.Exception)
            };

        /// <summary>
        /// Empties an optional, and attaches an exceptional value, 
        /// if the value is null.
        /// </summary>
        /// <param name="option">The specified optional.</param>
        /// <param name="exceptionFactory">A factory function to create an exceptional value to attach.</param>
        /// <returns>The filtered optional.</returns>
        public static Option<T, TException> NotDefault<T, TException>(this Option<T?, TException> option, Func<TException> exceptionFactory)
            where T : struct
        {
            if (exceptionFactory == null) throw new ArgumentNullException(nameof(exceptionFactory));
            return option switch
            {
                { HasValue: true, Value: null } => Option.None<T, TException>(exceptionFactory()),
                { HasValue: true, Value: { } value } =>Some<T, TException>(value),
                _ => Option.None<T, TException>(option.Exception)
            };
        }
    }
}
