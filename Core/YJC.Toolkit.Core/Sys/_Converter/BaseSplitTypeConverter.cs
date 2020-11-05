using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseSplitTypeConverter<T> : ITkTypeConverter where T : class, IEnumerable<string>
    {
        protected BaseSplitTypeConverter(char spliter)
        {
            Spliter = spliter;
            AutoTrim = true;
        }

        #region ITkTypeConverter 成员

        public string DefaultValue
        {
            get
            {
                return null;
            }
        }

        public object ConvertFromString(string text, ReadSettings settings)
        {
            if (text == null)
                return null;
            try
            {
                string[] data = text.Split(new char[] { Spliter }, StringSplitOptions.RemoveEmptyEntries);
                if (AutoTrim)
                {
                    for (int i = 0; i < data.Length; i++)
                        data[i] = data[i].Trim();
                }
                return Convert(data);
            }
            catch
            {
                return null;
            }
        }

        public string ConvertToString(object value, WriteSettings settings)
        {
            T data = value as T;
            if (data != null)
                return string.Join(Spliter.ToString(), data);
            return null;
        }

        #endregion ITkTypeConverter 成员

        public char Spliter { get; private set; }

        public bool AutoTrim { get; set; }

        protected abstract T Convert(string[] data);
    }
}