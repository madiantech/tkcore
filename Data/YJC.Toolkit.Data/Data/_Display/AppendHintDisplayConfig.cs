using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [DecorateDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "在显示的内容添加配置的Hint")]
    [ObjectContext]
    internal class AppendHintDisplayConfig : IDecorateDisplay, IConfigCreator<IDecorateDisplay>
    {
        #region IDecorateDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field,
            IFieldValueProvider rowValue, string linkedValue)
        {
            string result = linkedValue;
            string hint = field.Hint;
            if (string.IsNullOrEmpty(hint))
                return result;
            switch (Position)
            {
                case DisplayPosition.Head:
                    result = string.Format(ObjectUtil.SysCulture, "{0}{1}{2}",
                        hint, Seperator, result);
                    return result;
                default:
                    result += Seperator + hint;
                    return result;
            }
        }

        #endregion

        #region IConfigCreator<IDecorateDisplay> 成员

        public IDecorateDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        [SimpleAttribute(DefaultValue = DisplayPosition.End)]
        public DisplayPosition Position { get; private set; }

        [SimpleAttribute(DefaultValue = " ")]
        public string Seperator { get; private set; }
    }
}
