using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "数据是http地址，可以用这个配置，将显示地址的超链")]
    [ObjectContext]
    internal class HttpDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            if (DisplayUtil.IsNull(value))
                return string.Empty;


            string text = StringUtil.EscapeXmlString(value.ConvertToString());
            string url = text.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) ?
                text : "http://" + text;
            string target = string.IsNullOrEmpty(Target) ? string.Empty :
                string.Format(ObjectUtil.SysCulture, " target=\"{0}\"", Target);

            return string.Format(ObjectUtil.SysCulture, "<a href=\"{0}\"{1}>{2}</a>", url, target, text);
        }

        #endregion

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        [SimpleAttribute(DefaultValue = "_blank")]
        public string Target { get; private set; }
    }
}
