using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [DecorateDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "对显示加注Html的颜色")]
    [ObjectContext]
    internal class HtmlColorDisplayConfig : IDecorateDisplay, IConfigCreator<IDecorateDisplay>
    {
        #region IDecorateDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field,
            IFieldValueProvider rowValue, string linkedValue)
        {
            if (string.IsNullOrEmpty(Color))
                return linkedValue;

            return DisplayColorText(linkedValue, Color);
        }

        #endregion IDecorateDisplay 成员

        #region IConfigCreator<IDecorateDisplay> 成员

        public IDecorateDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IDecorateDisplay> 成员

        [SimpleAttribute(Required = true)]
        public string Color { get; private set; }

        internal static string DisplayColorText(string value, string color)
        {
            bool isClass = color.StartsWith(".");
            if (isClass)
                return string.Format(ObjectUtil.SysCulture,
                    "<span class='{0}'>{1}</span>", color.Substring(1), value);
            else
                return string.Format(ObjectUtil.SysCulture,
                    "<span style='color:{0}'>{1}</span>", color, value);
        }
    }
}