using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [CacheInstance]
    class CodeDataCodeTable : DataCodeTable
    {
        private readonly CodeTableAttribute fAttribute;

        public CodeDataCodeTable(CodeDataConfigItem config)
        {
            fAttribute = new CodeTableAttribute(config);
            if (config.RowList != null)
                foreach (var item in config.RowList)
                    Add(item.Convert());
        }

        public override BasePlugInAttribute Attribute
        {
            get
            {
                return fAttribute;
            }
        }
    }
}
