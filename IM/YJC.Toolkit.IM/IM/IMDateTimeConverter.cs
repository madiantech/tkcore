using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public sealed class IMDateTimeConverter : ITkTypeConverter
    {
        #region ITkTypeConverter 成员

        public string DefaultValue
        {
            get
            {
                return IMUtil.ToCreateTime(DateTime.Now).ToString(ObjectUtil.SysCulture);
            }
        }

        public object ConvertFromString(string text, ReadSettings settings)
        {
            try
            {
                int value = int.Parse(text, ObjectUtil.SysCulture);
                return IMUtil.ToDateTime(value);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public string ConvertToString(object value, WriteSettings settings)
        {
            try
            {
                DateTime date = (DateTime)value;
                return IMUtil.ToCreateTime(date).ToString(ObjectUtil.SysCulture);
            }
            catch
            {
                return DefaultValue;
            }
        }

        #endregion
    }
}
