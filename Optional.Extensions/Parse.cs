using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions
{
    public static class Parse
    {
        public static Option<byte> ToByte(string s)
        {
            byte result;
            return byte.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<byte> ToByte(string s, NumberStyles style, IFormatProvider provider)
        {
            byte result;
            return byte.TryParse(s, style, provider, out result) ? result.Some() : result.None();
        }

        public static Option<sbyte> ToSByte(string s)
        {
            sbyte result;
            return sbyte.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<sbyte> ToSByte(string s, NumberStyles style, IFormatProvider provider)
        {
            sbyte result;
            return sbyte.TryParse(s, style, provider, out result) ? result.Some() : result.None();
        }

        public static Option<short> ToShort(string s)
        {
            short result;
            return short.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<short> ToShort(string s, NumberStyles style, IFormatProvider provider)
        {
            short result;
            return short.TryParse(s, style, provider, out result) ? result.Some() : result.None();
        }

        public static Option<ushort> ToUShort(string s)
        {
            ushort result;
            return ushort.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<ushort> ToUShort(string s, NumberStyles style, IFormatProvider provider)
        {
            ushort result;
            return ushort.TryParse(s, style, provider, out result) ? result.Some() : result.None();
        }

        public static Option<int> ToInt(string s)
        {
            int result;
            return int.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<int> ToInt(string s, NumberStyles style, IFormatProvider provider)
        {
            int result;
            return int.TryParse(s, style, provider, out result) ? result.Some() : result.None();
        }

        public static Option<uint> ToUInt(string s)
        {
            uint result;
            return uint.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<uint> ToUInt(string s, NumberStyles style, IFormatProvider provider)
        {
            uint result;
            return uint.TryParse(s, style, provider, out result) ? result.Some() : result.None();
        }

        public static Option<long> ToLong(string s)
        {
            long result;
            return long.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<long> ToLong(string s, NumberStyles style, IFormatProvider provider)
        {
            long result;
            return long.TryParse(s, style, provider, out result) ? result.Some() : result.None();
        }

        public static Option<ulong> ToULong(string s)
        {
            ulong result;
            return ulong.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<ulong> ToULong(string s, NumberStyles style, IFormatProvider provider)
        {
            ulong result;
            return ulong.TryParse(s, style, provider, out result) ? result.Some() : result.None();
        }

        public static Option<float> ToFloat(string s)
        {
            float result;
            return float.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<float> ToFloat(string s, NumberStyles style, IFormatProvider provider)
        {
            float result;
            return float.TryParse(s, style, provider, out result) ? result.Some() : result.None();
        }

        public static Option<double> ToDouble(string s)
        {
            double result;
            return double.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<double> ToDouble(string s, NumberStyles style, IFormatProvider provider)
        {
            double result;
            return double.TryParse(s, style, provider, out result) ? result.Some() : result.None();
        }

        public static Option<decimal> ToDecimal(string s)
        {
            decimal result;
            return decimal.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<decimal> ToDecimal(string s, NumberStyles style, IFormatProvider provider)
        {
            decimal result;
            return decimal.TryParse(s, style, provider, out result) ? result.Some() : result.None();
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

        public static Option<DateTime> ToDateTime(string s, IFormatProvider provider, DateTimeStyles style)
        {
            DateTime result;
            return DateTime.TryParse(s, provider, style, out result) ? result.Some() : result.None();
        }

        public static Option<DateTime> ToDateTimeExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
        {
            DateTime result;
            return DateTime.TryParseExact(s, format, provider, style, out result) ? result.Some() : result.None();
        }

        public static Option<DateTime> ToDateTimeExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
        {
            DateTime result;
            return DateTime.TryParseExact(s, formats, provider, style, out result) ? result.Some() : result.None();
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

        public static Option<TimeSpan> ToTimeSpanExact(string s, string format, IFormatProvider provider, TimeSpanStyles style)
        {
            TimeSpan result;
            return TimeSpan.TryParseExact(s, format, provider, style, out result) ? result.Some() : result.None();
        }

        public static Option<TimeSpan> ToTimeSpanExact(string s, string[] formats, IFormatProvider provider, TimeSpanStyles style)
        {
            TimeSpan result;
            return TimeSpan.TryParseExact(s, formats, provider, style, out result) ? result.Some() : result.None();
        }

        public static Option<DateTimeOffset> ToDateTimeOffset(string s)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParse(s, out result) ? result.Some() : result.None();
        }

        public static Option<DateTimeOffset> ToDateTimeOffset(string s, IFormatProvider provider, DateTimeStyles style)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParse(s, provider, style, out result) ? result.Some() : result.None();
        }

        public static Option<DateTimeOffset> ToDateTimeOffsetExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParseExact(s, format, provider, style, out result) ? result.Some() : result.None();
        }

        public static Option<DateTimeOffset> ToDateTimeOffsetExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
        {
            DateTimeOffset result;
            return DateTimeOffset.TryParseExact(s, formats, provider, style, out result) ? result.Some() : result.None();
        }
    }
}
