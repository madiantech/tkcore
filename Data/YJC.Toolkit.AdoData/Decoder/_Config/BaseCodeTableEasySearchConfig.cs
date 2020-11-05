using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal abstract class BaseCodeTableEasySearchConfig : BaseXmlPlugInItem
    {
        protected BaseCodeTableEasySearchConfig()
        {
        }

        [SimpleAttribute(Required = true)]
        public string TableName { get; protected set; }

        [SimpleAttribute]
        public string Context { get; protected set; }

        [SimpleAttribute]
        public string OrderBy { get; protected set; }

        [SimpleAttribute]
        public string NameExpression { get; protected set; }

        [SimpleAttribute]
        public string DisplayNameExpression { get; protected set; }
    }
}
