using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "布尔显示，显示打勾，打叉")]
    [ObjectContext]
    internal class CheckedDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field,
            IFieldValueProvider rowValue)
        {
            if (DisplayUtil.IsNull(value))
                return string.Empty;

            string strValue = value.ToString();
            if (strValue == CheckValue)
                return "<i class='icon-ok'></i>";
            else
                return "<i class='icon-remove'></i>";
        }

        #endregion

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        [SimpleAttribute(DefaultValue = "1")]
        public string CheckValue { get; private set; }

        [SimpleAttribute(DefaultValue = "0")]
        public string UncheckValue { get; private set; }
    }
}
