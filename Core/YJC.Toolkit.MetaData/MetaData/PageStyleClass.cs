using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [TkTypeConverter(typeof(PageStyleTypeConverter))]
    public sealed class PageStyleClass : IPageStyle
    {
        public static readonly PageStyleClass Empty = new PageStyleClass(string.Empty);

        private PageStyleClass(PageStyle style)
        {
            TkDebug.AssertArgument(IsNormalStyle(style), "style",
                "Style不支持Custom，All和AllNoList", null);

            Style = style;
        }

        private PageStyleClass(string operation)
        {
            TkDebug.AssertArgumentNull(operation, "operation", null);

            Style = PageStyle.Custom;
            Operation = operation;
        }

        #region IPageStyle 成员

        public PageStyle Style { get; private set; }

        public string Operation { get; private set; }

        #endregion

        public override bool Equals(object obj)
        {
            IPageStyle style1 = obj as IPageStyle;
            if (style1 == null)
                return false;

            return MetaDataUtil.Equals(this, style1);
        }

        public override int GetHashCode()
        {
            if (Style == PageStyle.Custom)
                return Operation.GetHashCode();
            return Style.GetHashCode();
        }

        public override string ToString()
        {
            if (Style != PageStyle.Custom)
                return Style.ToString();
            else
                return "C" + Operation;
        }

        private static bool IsNormalStyle(PageStyle style)
        {
            switch (style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                case PageStyle.Delete:
                case PageStyle.Detail:
                case PageStyle.List:
                    return true;
                default:
                    return false;
            }
        }

        public static IPageStyle FromPageStyle(PageStyle style)
        {
            return new PageStyleClass(style);
        }

        public static IPageStyle FromString(string operation)
        {
            return new PageStyleClass(operation);
        }

        public static PageStyleClass FromStyle(IPageStyle style)
        {
            if (style == null)
                return null;
            if (style is PageStyleClass)
                return (PageStyleClass)style;
            if (IsNormalStyle(style.Style))
                return new PageStyleClass(style.Style);
            else
                return new PageStyleClass(style.Operation);
        }

        public static bool operator ==(PageStyleClass left, PageStyleClass right)
        {
            return ObjectUtil.Equals(left, right);
        }

        public static bool operator !=(PageStyleClass left, PageStyleClass right)
        {
            return !ObjectUtil.Equals(left, right);
        }

        public static explicit operator PageStyleClass(PageStyle style)
        {
            return new PageStyleClass(style);
        }

        public static explicit operator PageStyleClass(string operation)
        {
            return new PageStyleClass(operation);
        }
    }
}
