using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2018-04-18", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "当浮点高于或低于某个基准值时，以指定的颜色来显示")]
    internal class DoubleColorDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            if (DisplayUtil.IsNull(value))
                return string.Empty;

            double dblValue = value.Value<double>();
            string format = string.Format(ObjectUtil.SysCulture, "{{0:{0}}}", Format);
            string text = string.Format(ObjectUtil.SysCulture, format, dblValue);
            if (dblValue > BaseValue)
                return DisplayText(text, HighColor);
            else if (dblValue < BaseValue)
                return DisplayText(text, LowColor);
            else
                return DisplayText(text, EqualColor);
        }

        #endregion IDisplay 成员

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IDisplay> 成员

        [SimpleAttribute]
        public double BaseValue { get; private set; }

        [SimpleAttribute]
        public string HighColor { get; private set; }

        [SimpleAttribute]
        public string LowColor { get; private set; }

        [SimpleAttribute]
        public string EqualColor { get; private set; }

        [SimpleAttribute(DefaultValue = "0.00")]
        public string Format { get; private set; }

        private static string DisplayText(string text, string color)
        {
            if (string.IsNullOrEmpty(color))
                return text;

            return HtmlColorDisplayConfig.DisplayColorText(text, color);
        }
    }
}