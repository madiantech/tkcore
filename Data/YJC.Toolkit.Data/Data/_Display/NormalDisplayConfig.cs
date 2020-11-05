using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "正常显示")]
    [ObjectContext]
    internal class NormalDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            string text = value.ConvertToString();
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (EscapeString)
                text = StringUtil.EscapeXmlString(text);
            return text;
        }

        #endregion

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        [SimpleAttribute(DefaultValue = true)]
        public bool EscapeString { get; private set; }
    }
}
