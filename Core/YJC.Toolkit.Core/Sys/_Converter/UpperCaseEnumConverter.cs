using System;

namespace YJC.Toolkit.Sys
{
    public sealed class UpperCaseEnumConverter : BaseEnumConverter
    {
        public UpperCaseEnumConverter(Type enumType)
            : base(enumType)
        {
        }

        protected override string ConvertEnumString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            return input.ToUpper();
        }
    }
}
