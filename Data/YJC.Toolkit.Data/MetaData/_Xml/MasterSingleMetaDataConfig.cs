using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class MasterSingleMetaDataConfig : BaseMDSingleMetaDataConfig
    {
        [SimpleAttribute]
        public bool Main { get; protected set; }

        [SimpleAttribute(DefaultValue = TableShowStyle.None)]
        public TableShowStyle ListStyle { get; protected set; }
    }
}