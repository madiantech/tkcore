using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class OneToOneChildTableInfoConfig : ChildTableInfoConfig
    {
        [SimpleAttribute(DefaultValue = NoRecordHandler.None)]
        public NoRecordHandler NoRecordHandler { get; private set; }
    }
}