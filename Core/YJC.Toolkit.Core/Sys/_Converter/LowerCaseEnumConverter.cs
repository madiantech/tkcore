using System;

namespace YJC.Toolkit.Sys
{
    public sealed class LowerCaseEnumConverter : BaseEnumConverter
    {
        public LowerCaseEnumConverter(Type type)
            : base(type)
        {
        }

        protected override string ConvertEnumString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            return input.ToLower();
        }
    }
}
