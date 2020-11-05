using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    internal class PageStyleTypeConverter : BaseTypeConverter<PageStyleClass>
    {
        private static readonly HashSet<string> Styles = CreateStyles();
        private static HashSet<string> CreateStyles()
        {
            HashSet<string> result = new HashSet<string>();
            result.Add(PageStyle.Insert.ToString().ToUpperInvariant());
            result.Add(PageStyle.Update.ToString().ToUpperInvariant());
            result.Add(PageStyle.Detail.ToString().ToUpperInvariant());
            result.Add(PageStyle.List.ToString().ToUpperInvariant());
            result.Add(PageStyle.Delete.ToString().ToUpperInvariant());

            return result;
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            if (Styles.Contains(text.ToUpperInvariant()))
                return PageStyleClass.FromPageStyle(ObjectUtil.ParseEnum<PageStyle>(text, true));
            else if (text.StartsWith("C", StringComparison.OrdinalIgnoreCase))
                return PageStyleClass.FromString(text.Substring(1));
            else
                return PageStyleClass.FromString(text);
        }
    }
}
