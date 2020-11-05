using System;

namespace YJC.Toolkit.MetaData
{
    public interface IFieldControl
    {
        PageStyle DefaultShow { get; }

        ControlType GetControl(IPageStyle style);

        int GetOrder(IPageStyle style);

        Tuple<string, string> GetCustomControl(IPageStyle style);
    }
}