using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "日期显示")]
    [ObjectContext]
    internal class DateDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            if (DisplayUtil.IsNull(value) || string.IsNullOrEmpty(value.ToString()))
                return string.Empty;

            DateTime date = value.Value<DateTime>();
            return date.ToString(Format, ObjectUtil.SysCulture);
        }

        #endregion IDisplay 成员

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IDisplay> 成员

        [SimpleAttribute(DefaultValue = "yyyy-MM-dd")]
        public string Format { get; private set; }
    }
}