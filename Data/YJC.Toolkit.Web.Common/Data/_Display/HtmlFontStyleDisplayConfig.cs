using System.Drawing;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [DecorateDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "用Html的方式设置显示的文字字体")]
    [ObjectContext]
    internal class HtmlFontStyleDisplayConfig : IDecorateDisplay, IConfigCreator<IDecorateDisplay>
    {
        #region IDecorateDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field,
            IFieldValueProvider rowValue, string linkedValue)
        {
            switch (Style)
            {
                case FontStyle.Bold:
                    return string.Format(ObjectUtil.SysCulture,
                        "<span class='bold'>{0}</span>", linkedValue);

                case FontStyle.Italic:
                    return string.Format(ObjectUtil.SysCulture,
                        "<span class='italic'>{0}</span>", linkedValue);

                case FontStyle.Underline:
                    return string.Format(ObjectUtil.SysCulture,
                        "<span class='underline'>{0}</span>", linkedValue);

                case FontStyle.Strikeout:
                    return string.Format(ObjectUtil.SysCulture,
                        "<span style='text-decoration:line-through;'>{0}</span>", linkedValue);
                default:
                    return linkedValue;
            }
        }

        #endregion IDecorateDisplay 成员

        #region IConfigCreator<IDecorateDisplay> 成员

        public IDecorateDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IDecorateDisplay> 成员

        [SimpleAttribute(Required = true)]
        public FontStyle Style { get; private set; }
    }
}