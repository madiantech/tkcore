using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class EnumFieldValueAttribute : Attribute
    {
        public EnumFieldValueAttribute(string value)
        {
            TkDebug.AssertArgumentNull(value, "value", null);

            Value = value;
        }

        public string Value { get; private set; }
    }
}
