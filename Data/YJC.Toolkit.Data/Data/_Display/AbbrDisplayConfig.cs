using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "内容简写显示")]
    [ObjectContext]
    internal class AbbrDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            if (DisplayUtil.IsNull(value))
                return string.Empty;

            string text = value.ToString();
            int len = text.Length;
            if (len <= MaxLength)
                return StringUtil.EscapeXmlString(text);

            switch (DisplayMode)
            {
                case DisplayPosition.Middle:
                    text = string.Format(ObjectUtil.SysCulture, "{0}{1}{2}",
                        text.Substring(0, MaxLength - EndLength), AbbrString,
                        text.Substring(len - EndLength, EndLength));
                    break;
                default:
                    text = text.Substring(0, MaxLength) + AbbrString;
                    break;
            }
            return StringUtil.EscapeXmlString(text);
        }

        #endregion

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        [SimpleAttribute(DefaultValue = DisplayPosition.End)]
        public DisplayPosition DisplayMode { get; private set; }

        [SimpleAttribute(DefaultValue = 30)]
        public int MaxLength { get; private set; }

        [SimpleAttribute(DefaultValue = 2)]
        public int EndLength { get; private set; }

        [SimpleAttribute(DefaultValue = "...")]
        public string AbbrString { get; private set; }
    }
}
