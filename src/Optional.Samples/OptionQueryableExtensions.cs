using System;
using System.Linq;
using System.Linq.Expressions;

namespace Optional.Samples
{
    public static class OptionQueryableExtensions
    {
        /// <summary>
        /// Returns the first element of a sequence if such exists.
        /// </summary>
        /// <param name="source">The sequence to return the first element from.</param>
        /// <returns>An Option&lt;T&gt; instance containing the first element if present.</returns>
        public static Option<TSource> FirstOrNone<TSource>(this IQueryable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var result = source.Select(val => new { Value = val }).FirstOrDefault();
            return result != null ? result.Value.Some() : Option.None<TSource>();
        }

        /// <summary>
        /// Returns the first element of a sequence, satisfying a specified predicate, 
        /// if such exists.
        /// </summary>
        /// <param name="source">The sequence to return the first element from.</param>
        /// <param name="predicate">The predicate to filter by.</param>
        /// <returns>An Option&lt;T&gt; instance containing the first element if present.</returns>
        public static Option<TSource> FirstOrNone<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var result = source.Where(predicate).Select(val => new { Value = val }).FirstOrDefault();
            return result != null ? result.Value.Some() : Option.None<TSource>();
        }

        /// <summary>
        /// Returns the last element of a sequence if such exists.
        /// </summary>
        /// <param name="source">The sequence to return the last element from.</param>
        /// <returns>An Option&lt;T&gt; instance containing the last element if present.</returns>
        public static Option<TSource> LastOrNone<TSource>(this IQueryable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var result = source.Select(val => new { Value = val }).LastOrDefault();
            return result != null ? result.Value.Some() : Option.None<TSource>();
        }

        /// <summary>
        /// Returns the last element of a sequence, satisfying a specified predicate, 
        /// if such exists.
        /// </summary>
        /// <param name="source">The sequence to return the last element from.</param>
        /// <param name="predicate">The predicate to filter by.</param>
        /// <returns>An Option&lt;T&gt; instance containing the last element if present.</returns>
        public static Option<TSource> LastOrNone<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var result = source.Where(predicate).Select(val => new { Value = val }).LastOrDefault();
            return result != null ? result.Value.Some() : Option.None<TSource>();
        }

        /// <summary>
        /// Returns a single element from a sequence, if it exists 
        /// and is the only element in the sequence.
        /// </summary>
        /// <param name="source">The sequence to return the element from.</param>
        /// <returns>An Option&lt;T&gt; instance containing the element if present.</returns>
        public static Option<TSource> SingleOrNone<TSource>(this IQueryable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            try
            {
                var result = source.Select(val => new { Value = val }).SingleOrDefault();
                return result != null ? result.Value.Some() : Option.None<TSource>();
            }
            catch (InvalidOperationException)
            {
                return Option.None<TSource>();
            }
        }

        /// <summary>
        /// Returns a single element from a sequence, satisfying a specified predicate, 
        /// if it exists and is the only element in the sequence.
        /// </summary>
        /// <param name="source">The sequence to return the element from.</param>
        /// <param name="predicate">The predicate to filter by.</param>
        /// <returns>An Option&lt;T&gt; instance containing the element if present.</returns>
        public static Option<TSource> SingleOrNone<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            try
            {
                var result = source.Where(predicate).Select(val => new { Value = val }).SingleOrDefault();
                return result != null ? result.Value.Some() : Option.None<TSource>();
            }
            catch (InvalidOperationException)
            {
                return Option.None<TSource>();
            }
        }

        /// <summary>
        /// Returns an element at a specified position in a sequence if such exists.
        /// </summary>
        /// <param name="source">The sequence to return the element from.</param>
        /// <param name="index">The index in the sequence.</param>
        /// <returns>An Option&lt;T&gt; instance containing the element if found.</returns>
        public static Option<TSource> ElementAtOrNone<TSource>(this IQueryable<TSource> source, int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var result = source.Select(val => new { Value = val }).ElementAtOrDefault(index);
            return result != null ? result.Value.Some() : Option.None<TSource>();
        }
    }
}
