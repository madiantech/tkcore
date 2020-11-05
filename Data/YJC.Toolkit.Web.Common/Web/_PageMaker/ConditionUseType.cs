using System;

namespace YJC.Toolkit.Web
{
    [Flags]
    internal enum ConditionUseType
    {
        True = 1,
        Post = 2,
        OutputType = 4,
        Style = 8,
        QueryString = 0x10
    }
}
