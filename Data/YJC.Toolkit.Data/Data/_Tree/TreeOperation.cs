using System;

namespace YJC.Toolkit.Data
{
    [Flags]
    public enum TreeOperation
    {
        Base = 0,
        NewChild = 1,
        MoveUpDown = 2,
        MoveNode = 4
    }
}
