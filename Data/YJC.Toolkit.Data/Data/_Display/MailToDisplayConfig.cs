using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "数据是邮箱地址，可以用这个配置，将显示发邮件的超链")]
    internal class MailToDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            if (DisplayUtil.IsNull(value))
                return string.Empty;

            string text = StringUtil.EscapeXmlString(value.ConvertToString());
            return string.Format(ObjectUtil.SysCulture,
                "<a href=\"mailto://{0}\" target=\"_blank\">{0}</a>", text);
        }

        #endregion

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion
    }
}
