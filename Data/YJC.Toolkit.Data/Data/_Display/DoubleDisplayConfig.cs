using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "数字显示")]
    [ObjectContext]
    internal class DoubleDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            if (DisplayUtil.IsNull(value))
                return string.Empty;

            string format = string.Format(ObjectUtil.SysCulture, "{{0:{0}}}", Format);
            double dblValue = value.Value<double>();
            return string.Format(ObjectUtil.SysCulture, format, dblValue);
        }

        #endregion

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        [SimpleAttribute(DefaultValue = "0.00")]
        public string Format { get; private set; }
    }
}
