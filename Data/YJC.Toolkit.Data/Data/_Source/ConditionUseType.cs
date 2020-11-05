using System;

namespace YJC.Toolkit.Data
{
    [Flags]
    public enum ConditionUseType
    {
        True = 1,
        Post = 2,
        OutputType = 4,
        Style = 8,
        QueryString = 0x10,
        StyleStartsWith = 0x20
    }
}
