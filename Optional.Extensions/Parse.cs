using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Optional.Extensions
{
    public static class Parse
    {
        public static Option<byte> ToByte(string s)
        {
            byte result;
            return byte.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<byte> ToByte(string s, IFormatProvider provider, NumberStyles styles)
        {
            byte result;
            return byte.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        public static Option<sbyte> ToSByte(string s)
        {
            sbyte result;
            return sbyte.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<sbyte> ToSByte(string s, IFormatProvider provider, NumberStyles styles)
        {
            sbyte result;
            return sbyte.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        public static Option<short> ToShort(string s)
        {
            short result;
            return short.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<short> ToShort(string s, IFormatProvider provider, NumberStyles styles)
        {
            short result;
            return short.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        public static Option<ushort> ToUShort(string s)
        {
            ushort result;
            return ushort.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<ushort> ToUShort(string s, IFormatProvider provider, NumberStyles styles)
        {
            ushort result;
            return ushort.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        public static Option<int> ToInt(string s)
        {
            int result;
            return int.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<int> ToInt(string s, IFormatProvider provider, NumberStyles styles)
        {
            int result;
            return int.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        public static Option<uint> ToUInt(string s)
        {
            uint result;
            return uint.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<uint> ToUInt(string s, IFormatProvider provider, NumberStyles styles)
        {
            uint result;
            return uint.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        public static Option<long> ToLong(string s)
        {
            long result;
            return long.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<long> ToLong(string s, IFormatProvider provider, NumberStyles styles)
        {
            long result;
            return long.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        public static Option<ulong> ToULong(string s)
        {
            ulong result;
            return ulong.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<ulong> ToULong(string s, IFormatProvider provider, NumberStyles styles)
        {
            ulong result;
            return ulong.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        public static Option<float> ToFloat(string s)
        {
            float result;
            return float.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<float> ToFloat(string s, IFormatProvider provider, NumberStyles styles)
        {
            float result;
            return float.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        public static Option<double> ToDouble(string s)
        {
            double result;
            return double.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<double> ToDouble(string s, IFormatProvider provider, NumberStyles styles)
        {
            double result;
            return double.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        public static Option<decimal> ToDecimal(string s)
        {
            decimal result;
            return decimal.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<decimal> ToDecimal(string s, IFormatProvider provider, NumberStyles styles)
        {
            decimal result;
            return decimal.TryParse(s, styles, provider, out result) ? result.Some() : result.None();
        }

        public static Option<bool> ToBool(string s)
        {
            bool result;
            return bool.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<char> ToChar(string s)
        {
            char result;
            return char.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<Guid> ToGuid(string s)
        {
            Guid result;
            return Guid.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<TEnum> ToEnum<TEnum>(string s) where TEnum : struct
        {
            TEnum result;
            return Enum.TryParse<TEnum>(s, out result) ? result.Some() : result.None();
        }

        public static Option<TEnum> ToEnum<TEnum>(string s, bool ignoreCase) where TEnum : struct
        {
            TEnum result;
            return Enum.TryParse<TEnum>(s, ignoreCase, out result) ? result.Some() : result.None();
        }

        public static Option<DateTime> ToDateTime(string s)
        {
            DateTime result;
            return DateTime.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<DateTime> ToDateTime(string s, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTime result;
            return DateTime.TryParse(s, provider, styles, out result) ? result.Some() : result.None();
        }

        public static Option<DateTime> ToDateTimeExact(string s, string format, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTime result;
            return DateTime.TryParseExact(s, format, provider, styles, out result) ? result.Some() : result.None();
        }

        public static Option<DateTime> ToDateTimeExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTime result;
            return DateTime.TryParseExact(s, formats, provider, styles, out result) ? result.Some() : result.None();
        }

        public static Option<TimeSpan> ToTimeSpan(string s)
        {
            TimeSpan result;
            return TimeSpan.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<TimeSpan> ToTimeSpan(string s, IFormatProvider provider)
        {
            TimeSpan result;
            return TimeSpan.TryParse(s, provider, out result) ? result.Some() : result.None();
        }

        public static Option<TimeSpan> ToTimeSpanExact(string s, string format, IFormatProvider provider)
        {
            TimeSpan result;
            return TimeSpan.TryParseExact(s, format, provider, out result) ? result.Some() : result.None();
        }

        public static Option<TimeSpan> ToTimeSpanExact(string s, string[] formats, IFormatProvider provider)
        {
            TimeSpan result;
            return TimeSpan.TryParseExact(s, formats, provider, out result) ? result.Some() : result.None();
        }

        public static Option<TimeSpan> ToTimeSpanExact(string s, string format, IFormatProvider provider, TimeSpanStyles styles)
        {
            TimeSpan result;
            return TimeSpan.TryParseExact(s, format, provider, styles, out result) ? result.Some() : result.None();
        }

        public static Option<TimeSpan> ToTimeSpanExact(string s, string[] formats, IFormatProvider provider, TimeSpanStyles styles)
        {
            TimeSpan result;
            return TimeSpan.TryParseExact(s, formats, provider, styles, out result) ? result.Some() : result.None();
        }

        public static Option<DateTimeOffset> ToDateTimeOffset(string s)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<DateTimeOffset> ToDateTimeOffset(string s, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParse(s, provider, styles, out result) ? result.Some() : result.None();
        }

        public static Option<DateTimeOffset> ToDateTimeOffsetExact(string s, string format, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParseExact(s, format, provider, styles, out result) ? result.Some() : result.None();
        }

        public static Option<DateTimeOffset> ToDateTimeOffsetExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParseExact(s, formats, provider, styles, out result) ? result.Some() : result.None();
        }
    }
}
