using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [CodeTableConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-01-12",
        Author = "YJC", Description = "标准代码表，含有CODE_VALUE等五个字段。（CD_开始的表一般无需定义）")]
    [ObjectContext]
    internal class StandardCodeTableConfig : BaseXmlPlugInItem, IReadObjectCallBack
    {
        public const string BASE_CLASS = "Standard";

        #region IXmlPlugInItem 成员

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }

        #endregion IXmlPlugInItem 成员

        [SimpleAttribute(Required = true)]
        public string TableName { get; protected set; }

        [SimpleAttribute]
        public string Context { get; protected set; }

        [SimpleAttribute]
        public string OrderBy { get; protected set; }

        [SimpleAttribute]
        public string NameExpression { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem FilterSql { get; protected set; }

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (string.IsNullOrEmpty(RegName))
                RegName = TableName;
        }

        #endregion IReadObjectCallBack 成员
    }
}