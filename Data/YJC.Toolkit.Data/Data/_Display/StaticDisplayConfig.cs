using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "显示静态文本，无视字段的数值")]
    [ObjectContext]
    internal class StaticDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            if (string.IsNullOrEmpty(Content))
                return string.Empty;
            if (EscapeString)
                Content = StringUtil.EscapeXmlString(Content);

            return Content;
        }

        #endregion

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        [TextContent(Required = true)]
        public string Content { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool EscapeString { get; private set; }
    }
}
