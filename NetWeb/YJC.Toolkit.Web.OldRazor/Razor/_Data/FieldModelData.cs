using System;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public sealed class FieldModelData : RowModelData
    {
        public FieldModelData(DataRow row, IFieldInfoEx fieldInfo)
            : base(row)
        {
            TkDebug.AssertArgumentNull(fieldInfo, "fieldInfo", null);

            FieldInfo = fieldInfo;
        }

        public IFieldInfoEx FieldInfo { get; private set; }
    }
}
