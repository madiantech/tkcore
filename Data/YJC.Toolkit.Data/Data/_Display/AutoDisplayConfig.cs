using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2016-09-08", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "自动根据配置调用相应的Display进行显示，和不配置相当")]
    [ObjectContext]
    internal class AutoDisplayConfig : IConfigCreator<IDisplay>, IDisplay
    {
        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            IConfigCreator<IDisplay> display = null;
            switch (Style)
            {
                case AutoStyle.Detail:
                    display = field.GetListDisplay();
                    break;
                case AutoStyle.Edit:
                    display = field.GetNormalEditDisplay();
                    break;
            }
            if (display == null)
                display = Tk5FieldInfoEx.GetNormalDisplay(Style == AutoStyle.Detail);
            IDisplay displayObj = display.CreateObject();

            return displayObj == null ? string.Empty :
                displayObj.DisplayValue(value, field, rowValue);
        }

        #endregion

        [SimpleAttribute(DefaultValue = AutoStyle.Detail)]
        public AutoStyle Style { get; set; }
    }
}
