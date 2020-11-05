using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [DecorateDisplayConfig(Author = "YJC", CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Description = "用{0}替代要叠加的值，其他部分为静态文本。没开启宏时，直接写{0}，如果开启宏，要写{{0}}")]
    internal class FormatDisplayConfig : MarcoConfigItem, IDecorateDisplay, IConfigCreator<IDecorateDisplay>
    {
        #region IDecorateDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field,
            IFieldValueProvider rowValue, string linkedValue)
        {
            string text = Expression.Execute(this);
            return string.Format(ObjectUtil.SysCulture, text, linkedValue);
        }

        #endregion

        #region IConfigCreator<IDecorateDisplay> 成员

        public IDecorateDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion
    }
}
