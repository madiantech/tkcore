using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class RowModelData
    {
        public RowModelData(DataRow row)
        {
            TkDebug.AssertArgumentNull(row, "row", null);

            Row = row;
        }

        public DataRow Row { get; private set; }
    }
}
