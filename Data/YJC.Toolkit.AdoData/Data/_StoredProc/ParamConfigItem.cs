using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class ParamConfigItem
    {
        [SimpleAttribute(DefaultValue = ParameterDirection.Input)]
        public ParameterDirection Kind { get; private set; }

        [SimpleAttribute]
        public string Name { get; private set; }

        [SimpleAttribute(DefaultValue = TkDataType.String)]
        public TkDataType Type { get; private set; }

        [SimpleAttribute]
        public int Size { get; private set; }

        [ObjectElement]
        public MarcoConfigItem DefaultValue { get; private set; }
    }
}