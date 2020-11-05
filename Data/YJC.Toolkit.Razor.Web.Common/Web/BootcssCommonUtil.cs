using System.Collections.Generic;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static class BootcssCommonUtil
    {
        private static readonly string[] BUTTON_ARRAY = new string[] { "btn-default",
            "btn-primary", "btn-success", "btn-info", "btn-warning", "btn-danger", "btn-link" };

        internal static string GetTotalClass(string @class, BootcssButton buttonType, bool block)
        {
            string buttonClass = BUTTON_ARRAY[(int)buttonType];
            string blockClass = block ? "btn-block" : string.Empty;
            string totalClass = HtmlCommonUtil.MergeClass("btn", buttonClass, blockClass, @class);
            return totalClass;
        }

        public static string Button(string caption, string @class, BootcssButton buttonType, bool block)
        {
            TkDebug.AssertArgumentNullOrEmpty(caption, "caption", null);

            string totalClass = GetTotalClass(@class, buttonType, block);
            return string.Format(ObjectUtil.SysCulture,
                "<button type=\"button\" class=\"{1}\">{0}</button>", caption, totalClass);
        }

        public static string Button(string caption, string @class)
        {
            return Button(caption, @class, BootcssButton.Default, false);
        }

        public static string Button(string caption)
        {
            return Button(caption, null, BootcssButton.Default, false);
        }

        public static string Button(string caption, string @class, BootcssButton buttonType, bool block,
            IEnumerable<HtmlAttribute> attributes)
        {
            TkDebug.AssertArgumentNullOrEmpty(caption, "caption", null);
            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            string totalClass = GetTotalClass(@class, buttonType, block);
            builder.Add("type", "button");
            builder.Add("class", totalClass);
            builder.AddRange(attributes);

            return string.Format(ObjectUtil.SysCulture, "<button {1}>{0}</button>", caption,
                builder.CreateAttribute());
        }

        public static string Button(string caption, string @class, BootcssButton buttonType, bool block,
            params HtmlAttribute[] attributes)
        {
            return Button(caption, @class, buttonType, block, (IEnumerable<HtmlAttribute>)attributes);
        }

        public static string CreateExcelButton()
        {
            return Button("<i class=\"icon-table mr5\"></i>导出Excel", "mr10 export-excel", BootcssButton.Default, false, null);
        }

        public static string GetColClass(int col)
        {
            TkDebug.AssertArgument(col > 0 && col <= 12, "col", string.Format(ObjectUtil.SysCulture,
                "col必须在1到12之间，当前值是{0}越界了", col), null);

            string result = string.Format(ObjectUtil.SysCulture,
                "col-xs-{0} col-sm-{0} col-md-{0} col-lg-{0}", col);
            return result;
        }

        public static string EditCaption(string nickName, string caption, int col, string @class)
        {
            return string.Format(ObjectUtil.SysCulture,
                "<label for=\"{0}\" class=\"{1}\">{2}</label>", nickName,
                HtmlCommonUtil.MergeClass(GetColClass(col), @class), caption);
        }

        public static string GetContainerClass(PageContainer container)
        {
            switch (container)
            {
                case PageContainer.Container:
                    return "container";

                case PageContainer.Fluid:
                default:
                    return "container-fluid";
            }
        }

        public static string GetTabClass(TabDisplayType displayType)
        {
            switch (displayType)
            {
                case TabDisplayType.Pill:
                    return "nav-pills";

                case TabDisplayType.NormalJustified:
                    return "nav-tabs nav-justified";

                case TabDisplayType.PillJustified:
                    return "nav-pills nav-justified";

                default:
                    return "nav-tabs";
            }
        }

        public static string CreateTabSheets(IEnumerable<ListTabSheet> tabSheets)
        {
            if (tabSheets == null)
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            foreach (var sheet in tabSheets)
            {
                string active = string.Empty;
                if (sheet.Selected)
                    active = " class=\"active\"";
                builder.AppendFormat(ObjectUtil.SysCulture,
                    "<li role=\"presentation\"{0}><a href=\"#\" data-tab=\"{1}\">{2}</a></li>\n",
                    active, sheet.Id, sheet.Caption);
            }
            return builder.ToString();
        }

        public static string ShowSearchCheckBox(SearchDataMethod method)
        {
            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            builder.Add("type", "checkbox");
            builder.Add("id", "_searchMethod");
            string caption;
            if (((int)method & 4) == 4)
            {
                builder.Add("class", "vam");
                caption = "精确";
            }
            else
            {
                builder.Add("class", "hide");
                caption = string.Empty;
            }
            if ((method & SearchDataMethod.Exactly) == SearchDataMethod.Exactly)
                builder.Add((HtmlAttribute)"checked");
            return string.Format(ObjectUtil.SysCulture, Html.CheckBox, builder.CreateAttribute(), caption);
        }
    }
}