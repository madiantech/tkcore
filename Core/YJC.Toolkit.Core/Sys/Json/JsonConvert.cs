using System;
using System.IO;

namespace YJC.Toolkit.Sys.Json
{
    /// <summary>
    /// Provides methods for converting between common language runtime types and JSON types.
    /// </summary>
    internal static class JsonConvert
    {
        /// <summary>
        /// Represents JavaScript's boolean value true as a string. This field is read-only.
        /// </summary>
        public static readonly string True = "true";

        /// <summary>
        /// Represents JavaScript's boolean value false as a string. This field is read-only.
        /// </summary>
        public static readonly string False = "false";

        /// <summary>
        /// Represents JavaScript's null as a string. This field is read-only.
        /// </summary>
        public static readonly string Null = "null";

        /// <summary>
        /// Represents JavaScript's undefined as a string. This field is read-only.
        /// </summary>
        public static readonly string Undefined = "undefined";

        /// <summary>
        /// Represents JavaScript's positive infinity as a string. This field is read-only.
        /// </summary>
        public static readonly string PositiveInfinity = "Infinity";

        /// <summary>
        /// Represents JavaScript's negative infinity as a string. This field is read-only.
        /// </summary>
        public static readonly string NegativeInfinity = "-Infinity";

        /// <summary>
        /// Represents JavaScript's NaN as a string. This field is read-only.
        /// </summary>
        public static readonly string NaN = "NaN";

        internal static readonly long InitialJavaScriptDateTicks = 621355968000000000;

        /// <summary>
        /// Converts the <see cref="DateTime"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="DateTime"/>.</returns>
        public static string ToString(DateTime value)
        {
            using (StringWriter writer = StringUtil.CreateStringWriter(64))
            {
                WriteDateTimeString(writer, value, GetUtcOffset(value), value.Kind);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Converts the <see cref="DateTimeOffset"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="DateTimeOffset"/>.</returns>
        public static string ToString(DateTimeOffset value)
        {
            using (StringWriter writer = StringUtil.CreateStringWriter(64))
            {
                WriteDateTimeString(writer, value.UtcDateTime, value.Offset, DateTimeKind.Local);
                return writer.ToString();
            }
        }

        private static TimeSpan GetUtcOffset(DateTime dateTime)
        {
            //return TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
            return TimeZoneInfo.Local.GetUtcOffset(dateTime);
        }

        internal static void WriteDateTimeString(TextWriter writer, DateTime value)
        {
            WriteDateTimeString(writer, value, GetUtcOffset(value), value.Kind);
        }

        internal static void WriteDateTimeString(TextWriter writer, DateTime value, TimeSpan offset, DateTimeKind kind)
        {
            long javaScriptTicks = ConvertDateTimeToJavaScriptTicks(value, offset);

            writer.Write(@"""\/Date(");
            writer.Write(javaScriptTicks);

            switch (kind)
            {
                case DateTimeKind.Local:
                case DateTimeKind.Unspecified:
                    writer.Write((offset.Ticks >= 0L) ? "+" : "-");

                    int absHours = Math.Abs(offset.Hours);
                    if (absHours < 10)
                        writer.Write(0);
                    writer.Write(absHours);
                    int absMinutes = Math.Abs(offset.Minutes);
                    if (absMinutes < 10)
                        writer.Write(0);
                    writer.Write(absMinutes);
                    break;
            }

            writer.Write(@")\/""");
        }

        private static long ToUniversalTicks(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime.Ticks;

            return ToUniversalTicks(dateTime, GetUtcOffset(dateTime));
        }

        private static long ToUniversalTicks(DateTime dateTime, TimeSpan offset)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime.Ticks;

            long ticks = dateTime.Ticks - offset.Ticks;
            if (ticks > 3155378975999999999L)
                return 3155378975999999999L;

            if (ticks < 0L)
                return 0L;

            return ticks;
        }

        internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, TimeSpan offset)
        {
            long universialTicks = ToUniversalTicks(dateTime, offset);

            return UniversialTicksToJavaScriptTicks(universialTicks);
        }

        internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
        {
            return ConvertDateTimeToJavaScriptTicks(dateTime, true);
        }

        internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, bool convertToUtc)
        {
            long ticks = (convertToUtc) ? ToUniversalTicks(dateTime) : dateTime.Ticks;

            return UniversialTicksToJavaScriptTicks(ticks);
        }

        private static long UniversialTicksToJavaScriptTicks(long universialTicks)
        {
            long javaScriptTicks = (universialTicks - InitialJavaScriptDateTicks) / 10000;

            return javaScriptTicks;
        }

        internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
        {
            DateTime dateTime = new DateTime((javaScriptTicks * 10000) + InitialJavaScriptDateTicks, DateTimeKind.Utc);

            return dateTime;
        }

        /// <summary>
        /// Converts the <see cref="Boolean"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Boolean"/>.</returns>
        public static string ToString(bool value)
        {
            return (value) ? True : False;
        }

        /// <summary>
        /// Converts the <see cref="Char"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Char"/>.</returns>
        public static string ToString(char value)
        {
            return ToString(char.ToString(value));
        }

        /// <summary>
        /// Converts the <see cref="Enum"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Enum"/>.</returns>
        public static string ToString(Enum value)
        {
            return value.ToString("D");
        }

        /// <summary>
        /// Converts the <see cref="Int32"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Int32"/>.</returns>
        public static string ToString(int value)
        {
            return value.ToString(null, ObjectUtil.SysCulture);
        }

        /// <summary>
        /// Converts the <see cref="Int16"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Int16"/>.</returns>
        public static string ToString(short value)
        {
            return value.ToString(null, ObjectUtil.SysCulture);
        }

        /// <summary>
        /// Converts the <see cref="UInt16"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="UInt16"/>.</returns>
        public static string ToString(ushort value)
        {
            return value.ToString(null, ObjectUtil.SysCulture);
        }

        /// <summary>
        /// Converts the <see cref="UInt32"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="UInt32"/>.</returns>
        public static string ToString(uint value)
        {
            return value.ToString(null, ObjectUtil.SysCulture);
        }

        /// <summary>
        /// Converts the <see cref="Int64"/>  to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Int64"/>.</returns>
        public static string ToString(long value)
        {
            return value.ToString(null, ObjectUtil.SysCulture);
        }

        /// <summary>
        /// Converts the <see cref="UInt64"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="UInt64"/>.</returns>
        public static string ToString(ulong value)
        {
            return value.ToString(null, ObjectUtil.SysCulture);
        }

        /// <summary>
        /// Converts the <see cref="Single"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Single"/>.</returns>
        public static string ToString(float value)
        {
            return EnsureDecimalPlace(value, value.ToString("R", ObjectUtil.SysCulture));
        }

        /// <summary>
        /// Converts the <see cref="Double"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Double"/>.</returns>
        public static string ToString(double value)
        {
            return EnsureDecimalPlace(value, value.ToString("R", ObjectUtil.SysCulture));
        }

        private static string EnsureDecimalPlace(double value, string text)
        {
            if (double.IsNaN(value) || double.IsInfinity(value) || text.IndexOf('.') != -1 || text.IndexOf('E') != -1)
                return text;

            return text + ".0";
        }

        private static string EnsureDecimalPlace(string text)
        {
            if (text.IndexOf('.') != -1)
                return text;

            return text + ".0";
        }

        /// <summary>
        /// Converts the <see cref="Byte"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Byte"/>.</returns>
        public static string ToString(byte value)
        {
            return value.ToString(null, ObjectUtil.SysCulture);
        }

        /// <summary>
        /// Converts the <see cref="SByte"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="SByte"/>.</returns>
        public static string ToString(sbyte value)
        {
            return value.ToString(null, ObjectUtil.SysCulture);
        }

        /// <summary>
        /// Converts the <see cref="Decimal"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="SByte"/>.</returns>
        public static string ToString(decimal value)
        {
            return EnsureDecimalPlace(value.ToString(null, ObjectUtil.SysCulture));
        }

        /// <summary>
        /// Converts the <see cref="Guid"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Guid"/>.</returns>
        public static string ToString(Guid value)
        {
            return '"' + value.ToString("D") + '"';
        }

        /// <summary>
        /// Converts the <see cref="String"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="String"/>.</returns>
        public static string ToString(string value)
        {
            return ToString(value, '"');
        }

        /// <summary>
        /// Converts the <see cref="String"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="delimter">The string delimiter character.</param>
        /// <returns>A JSON string representation of the <see cref="String"/>.</returns>
        public static string ToString(string value, char delimter)
        {
            return JavaScriptUtil.ToEscapedJavaScriptString(value, delimter, true);
        }

        /// <summary>
        /// Converts the <see cref="Object"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Object"/>.</returns>
        public static string ToString(object value)
        {
            if (value == null)
                return Null;

            IConvertible convertible = value as IConvertible;

            if (convertible != null)
            {
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.String:
                        return ToString(convertible.ToString(ObjectUtil.SysCulture));
                    case TypeCode.Char:
                        return ToString(convertible.ToChar(ObjectUtil.SysCulture));
                    case TypeCode.Boolean:
                        return ToString(convertible.ToBoolean(ObjectUtil.SysCulture));
                    case TypeCode.SByte:
                        return ToString(convertible.ToSByte(ObjectUtil.SysCulture));
                    case TypeCode.Int16:
                        return ToString(convertible.ToInt16(ObjectUtil.SysCulture));
                    case TypeCode.UInt16:
                        return ToString(convertible.ToUInt16(ObjectUtil.SysCulture));
                    case TypeCode.Int32:
                        return ToString(convertible.ToInt32(ObjectUtil.SysCulture));
                    case TypeCode.Byte:
                        return ToString(convertible.ToByte(ObjectUtil.SysCulture));
                    case TypeCode.UInt32:
                        return ToString(convertible.ToUInt32(ObjectUtil.SysCulture));
                    case TypeCode.Int64:
                        return ToString(convertible.ToInt64(ObjectUtil.SysCulture));
                    case TypeCode.UInt64:
                        return ToString(convertible.ToUInt64(ObjectUtil.SysCulture));
                    case TypeCode.Single:
                        return ToString(convertible.ToSingle(ObjectUtil.SysCulture));
                    case TypeCode.Double:
                        return ToString(convertible.ToDouble(ObjectUtil.SysCulture));
                    case TypeCode.DateTime:
                        return ToString(convertible.ToDateTime(ObjectUtil.SysCulture));
                    case TypeCode.Decimal:
                        return ToString(convertible.ToDecimal(ObjectUtil.SysCulture));
                    //case TypeCode.DBNull:
                    //    return Null;
                }
            }
            else if (value is DateTimeOffset)
            {
                return ToString((DateTimeOffset)value);
            }

            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                "不支持的类型: {0}", value.GetType()), null);
            return null;
        }

    }
}