using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Optional.Utilities
{
    /// <summary>
    /// A collection of static helper methods, for parsing strings into simple types.
    /// </summary>
    public static class Parse
    {
        /// <summary>
        /// Tries to parse a string into a byte.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<byte> ToByte(string s)
        {
            byte result;
            return byte.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a byte.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<byte> ToByte(string s, IFormatProvider provider, NumberStyles styles)
        {
            byte result;
            return byte.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a signed byte.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        [CLSCompliant(false)]
        public static Option<sbyte> ToSByte(string s)
        {
            sbyte result;
            return sbyte.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a signed byte.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        [CLSCompliant(false)]
        public static Option<sbyte> ToSByte(string s, IFormatProvider provider, NumberStyles styles)
        {
            sbyte result;
            return sbyte.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a short.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<short> ToShort(string s)
        {
            short result;
            return short.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a short.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<short> ToShort(string s, IFormatProvider provider, NumberStyles styles)
        {
            short result;
            return short.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into an unsigned short.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        [CLSCompliant(false)]
        public static Option<ushort> ToUShort(string s)
        {
            ushort result;
            return ushort.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into an unsigned short.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        [CLSCompliant(false)]
        public static Option<ushort> ToUShort(string s, IFormatProvider provider, NumberStyles styles)
        {
            ushort result;
            return ushort.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into an int.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<int> ToInt(string s)
        {
            int result;
            return int.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into an int.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<int> ToInt(string s, IFormatProvider provider, NumberStyles styles)
        {
            int result;
            return int.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into an unsigned int.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        [CLSCompliant(false)]
        public static Option<uint> ToUInt(string s)
        {
            uint result;
            return uint.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into an unsigned int.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        [CLSCompliant(false)]
        public static Option<uint> ToUInt(string s, IFormatProvider provider, NumberStyles styles)
        {
            uint result;
            return uint.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a long.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<long> ToLong(string s)
        {
            long result;
            return long.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a long.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<long> ToLong(string s, IFormatProvider provider, NumberStyles styles)
        {
            long result;
            return long.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into an unsigned long.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        [CLSCompliant(false)]
        public static Option<ulong> ToULong(string s)
        {
            ulong result;
            return ulong.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into an unsigned long.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        [CLSCompliant(false)]
        public static Option<ulong> ToULong(string s, IFormatProvider provider, NumberStyles styles)
        {
            ulong result;
            return ulong.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a float.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<float> ToFloat(string s)
        {
            float result;
            return float.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a float.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<float> ToFloat(string s, IFormatProvider provider, NumberStyles styles)
        {
            float result;
            return float.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a double.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<double> ToDouble(string s)
        {
            double result;
            return double.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a double.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<double> ToDouble(string s, IFormatProvider provider, NumberStyles styles)
        {
            double result;
            return double.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a decimal.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<decimal> ToDecimal(string s)
        {
            decimal result;
            return decimal.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a decimal.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<decimal> ToDecimal(string s, IFormatProvider provider, NumberStyles styles)
        {
            decimal result;
            return decimal.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a bool.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<bool> ToBool(string s)
        {
            bool result;
            return bool.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a char.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<char> ToChar(string s)
        {
            char result;
            return char.TryParse(s, out result) ? result.Some() : result.None();
        }

#if NET4PLUS
        /// <summary>
        /// Tries to parse a string into a guid.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<Guid> ToGuid(string s)
        {
            Guid result;
            return Guid.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into an enum.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<TEnum> ToEnum<TEnum>(string s) where TEnum : struct
        {
            TEnum result;
            return Enum.TryParse<TEnum>(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into an enum.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<TEnum> ToEnum<TEnum>(string s, bool ignoreCase) where TEnum : struct
        {
            TEnum result;
            return Enum.TryParse<TEnum>(s, ignoreCase, out result) ? result.Some() : result.None();
        }
#endif

        /// <summary>
        /// Tries to parse a string into a datetime.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<DateTime> ToDateTime(string s)
        {
            DateTime result;
            return DateTime.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a datetime.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<DateTime> ToDateTime(string s, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTime result;
            return DateTime.TryParse(s, provider, styles, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a datetime with a specific format.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<DateTime> ToDateTimeExact(string s, string format, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTime result;
            return DateTime.TryParseExact(s, format, provider, styles, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a datetime with a specific format.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<DateTime> ToDateTimeExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTime result;
            return DateTime.TryParseExact(s, formats, provider, styles, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a timespan.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<TimeSpan> ToTimeSpan(string s)
        {
            TimeSpan result;
            return TimeSpan.TryParse(s, out result) ? result.Some() : result.None();
        }

#if NET4PLUS
        /// <summary>
        /// Tries to parse a string into a timespan.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<TimeSpan> ToTimeSpan(string s, IFormatProvider provider)
        {
            TimeSpan result;
            return TimeSpan.TryParse(s, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a timespan with a specific format.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<TimeSpan> ToTimeSpanExact(string s, string format, IFormatProvider provider)
        {
            TimeSpan result;
            return TimeSpan.TryParseExact(s, format, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a timespan with a specific format.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<TimeSpan> ToTimeSpanExact(string s, string[] formats, IFormatProvider provider)
        {
            TimeSpan result;
            return TimeSpan.TryParseExact(s, formats, provider, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a timespan with a specific format.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<TimeSpan> ToTimeSpanExact(string s, string format, IFormatProvider provider, TimeSpanStyles styles)
        {
            TimeSpan result;
            return TimeSpan.TryParseExact(s, format, provider, styles, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a timespan with a specific format.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<TimeSpan> ToTimeSpanExact(string s, string[] formats, IFormatProvider provider, TimeSpanStyles styles)
        {
            TimeSpan result;
            return TimeSpan.TryParseExact(s, formats, provider, styles, out result) ? result.Some() : result.None();
        }
#endif

        /// <summary>
        /// Tries to parse a string into a datetime offset.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<DateTimeOffset> ToDateTimeOffset(string s)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParse(s, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a datetime offset.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<DateTimeOffset> ToDateTimeOffset(string s, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParse(s, provider, styles, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a datetime offset with a specific format.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<DateTimeOffset> ToDateTimeOffsetExact(string s, string format, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParseExact(s, format, provider, styles, out result) ? result.Some() : result.None();
        }

        /// <summary>
        /// Tries to parse a string into a datetime offset with a specific format.
        /// </summary>
        /// <returns>An optional value containing the result if any.</returns>
        public static Option<DateTimeOffset> ToDateTimeOffsetExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParseExact(s, formats, provider, styles, out result) ? result.Some() : result.None();
        }
    }
}
