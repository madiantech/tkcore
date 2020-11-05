using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class ObjectModelData
    {
        public ObjectModelData(ObjectContainer data)
        {
            TkDebug.AssertArgumentNull(data, "data", null);

            Data = data;
        }

        public ObjectContainer Data { get; private set; }
    }
}
