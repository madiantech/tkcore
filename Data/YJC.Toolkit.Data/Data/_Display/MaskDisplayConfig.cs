using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "加密显示内容")]
    [ObjectContext]
    internal class MaskDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            if (DisplayUtil.IsNull(value))
                return string.Empty;

            string text = value.ToString();
            int length = FixLength == 0 ? text.Length : FixLength;
            int txtLength = text.Length;
            int controllLength = length > txtLength ? txtLength : length;
            string result;

            int headLength = HeadLength;
            int endLength = EndLength;
            switch (DisplayMode)
            {
                case DisplayPosition.All:
                    result = string.Empty.PadLeft(length, MaskChar);
                    break;

                case DisplayPosition.Head:
                    if (headLength >= controllLength)
                        headLength = 0;
                    result = string.Format(ObjectUtil.SysCulture, "{0}{1}",
                        text.Substring(0, headLength),
                        string.Empty.PadLeft(length - headLength, MaskChar));
                    break;

                case DisplayPosition.Middle:
                    if (headLength + endLength >= controllLength)
                    {
                        endLength = 0;
                        if (headLength >= controllLength)
                            headLength = 0;
                    }
                    result = string.Format(ObjectUtil.SysCulture, "{0}{1}{2}",
                        text.Substring(0, headLength),
                        string.Empty.PadLeft(length - headLength - endLength, MaskChar),
                        text.Substring(txtLength - endLength, endLength));
                    break;

                case DisplayPosition.End:
                    if (endLength >= controllLength)
                        endLength = 0;
                    result = string.Format(ObjectUtil.SysCulture, "{0}{1}",
                        string.Empty.PadLeft(length - endLength, MaskChar),
                        text.Substring(txtLength - endLength, endLength));
                    break;

                default:
                    result = string.Empty;
                    break;
            }

            return StringUtil.EscapeXmlString(result);
        }

        #endregion IDisplay 成员

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IDisplay> 成员

        [SimpleAttribute(DefaultValue = DisplayPosition.All)]
        public DisplayPosition DisplayMode { get; private set; }

        [SimpleAttribute]
        public int FixLength { get; private set; }

        [SimpleAttribute(DefaultValue = 2)]
        public int HeadLength { get; private set; }

        [SimpleAttribute(DefaultValue = 2)]
        public int EndLength { get; private set; }

        [SimpleAttribute(DefaultValue = '*')]
        public char MaskChar { get; private set; }
    }
}