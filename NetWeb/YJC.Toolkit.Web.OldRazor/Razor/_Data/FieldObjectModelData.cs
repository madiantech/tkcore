using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class FieldObjectModelData : ObjectModelData
    {
        /// <summary>
        /// Initializes a new instance of the FieldObjectModelData class.
        /// </summary>
        public FieldObjectModelData(ObjectContainer data, IFieldInfoEx fieldInfo)
            : base(data)
        {
            TkDebug.AssertArgumentNull(fieldInfo, "fieldInfo", null);

            FieldInfo = fieldInfo;
        }

        public IFieldInfoEx FieldInfo { get; private set; }
    }
}
