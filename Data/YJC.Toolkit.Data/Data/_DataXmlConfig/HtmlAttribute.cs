using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Serializable]
    public class HtmlAttribute : IRegName
    {
        public HtmlAttribute()
        {
        }

        public HtmlAttribute(string name, object value)
        {
            Name = name;
            Value = value == null ? null : value.ToString();
        }

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return Name;
            }
        }

        #endregion

        [SimpleAttribute]
        public string Name { get; set; }

        [SimpleAttribute]
        public string Value { get; set; }

        public static explicit operator HtmlAttribute(string name)
        {
            return new HtmlAttribute { Name = name };
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
                return string.Empty;
            if (string.IsNullOrEmpty(Value))
                return Name;
            return string.Format(ObjectUtil.SysCulture, "{0}=\"{1}\"", Name,
                StringUtil.EscapeHtmlAttribute(Value));
        }
    }
}