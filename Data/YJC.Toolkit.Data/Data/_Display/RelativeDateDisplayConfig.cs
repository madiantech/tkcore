using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2018-04-22", NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        Description = "根据当前日期和显示日期比较，酌情显示今天，明天，昨天，上溯一周内显示星期几，其他按照Format显示")]
    [ObjectContext]
    internal class RelativeDateDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            if (DisplayUtil.IsNull(value) || string.IsNullOrEmpty(value.ToString()))
                return string.Empty;

            DateTime today = DateTime.Today;
            DateTime dateValue = value.Value<DateTime>();
            if (today == dateValue)
                return TkWebApp.Today;
            else if (today.AddDays(1) == dateValue)
                return TkWebApp.Tomorrow;
            else if (today.AddDays(-1) == dateValue)
                return TkWebApp.Yesterday;
            else
            {
                DateTime weekDate = today.AddDays(-6);
                if (dateValue > weekDate && dateValue < today)
                    return dateValue.ToString("dddd", ObjectUtil.SysCulture);
                else
                    return dateValue.ToString(Format, ObjectUtil.SysCulture);
            }
        }

        #endregion IDisplay 成员

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IDisplay> 成员

        [SimpleAttribute(DefaultValue = "yy-MM-dd")]
        public string Format { get; private set; }
    }
}