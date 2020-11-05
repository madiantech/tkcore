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

        private static string GetTotalClass(string @class, BootcssButton buttonType, bool block)
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

        private static string ListButtonFromConfig(Operator optr, IFieldValueProvider receiver)
        {
            string icon;
            if (!string.IsNullOrEmpty(optr.IconClass))
                icon = string.Format(ObjectUtil.SysCulture, "<i class=\"{0} mr5\"></i>",
                    optr.IconClass);
            else
                icon = string.Empty;
            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            string totalClass = GetTotalClass("mr10", BootcssButton.Default, false);
            builder.Add("type", "button");
            builder.Add("class", totalClass);

            string info = optr.Info ?? string.Empty;
            string attrUrl;
            bool isJs = info.Contains("JS");
            if (isJs)
            {
                string js = "javascript:" + HtmlCommonUtil.ResolveString(receiver,
                    optr.Content);
                builder.Add("onClick", js);
            }
            else
            {
                if (info.Contains("Dialog"))
                {
                    attrUrl = "data-dialog-url";
                    string dialogTitle = HtmlCommonUtil.ResolveString(receiver, optr.DialogTitle);
                    builder.Add("data-title", dialogTitle);
                }
                else if (info.Contains("AjaxUrl"))
                {
                    attrUrl = "data-ajax-url";
                    if (info.Contains("Post"))
                        builder.Add("data-method", "post");
                }
                else
                    attrUrl = "data-url";
                builder.Add(attrUrl, HtmlCommonUtil.ParseLinkUrl(receiver, optr.Content));
                if (!string.IsNullOrEmpty(optr.ConfirmData))
                    builder.Add("data-confirm", optr.ConfirmData);
            }

            string button = string.Format(ObjectUtil.SysCulture, "<button {1}>{2}{0}</button>",
                optr.Caption, builder.CreateAttribute(), icon);

            return button;
        }

        public static string CreateOperators(IEnumerable<Operator> operators, IFieldValueProvider receiver)
        {
            if (operators == null)
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            foreach (var item in operators)
                builder.Append(ListButtonFromConfig(item, receiver));

            return builder.ToString();
        }

        private static string CreateRowOperator(IFieldValueProvider row, HtmlAttributeBuilder attrBuilder, Operator operRow)
        {
            string caption = operRow.Caption;
            string urlContent = operRow.Content;
            if (string.IsNullOrEmpty(urlContent))
                return string.Format(ObjectUtil.SysCulture, "<li>{0}</li>", caption);
            else
            {
                attrBuilder.Clear();
                attrBuilder.Add("href", "#");
                string info = operRow.Info ?? string.Empty;
                bool isJs = info.Contains("JS");
                if (isJs)
                {
                    string js = "javascript:" + HtmlCommonUtil.ResolveString(row, urlContent);
                    attrBuilder.Add("onClick", js);
                }
                else
                {
                    string attrUrl;
                    if (info.Contains("Dialog"))
                    {
                        string dialogTitle = HtmlCommonUtil.ResolveString(row, operRow.DialogTitle);
                        attrBuilder.Add("data-title", dialogTitle);
                        attrUrl = "data-dialog-url";
                    }
                    else if (info.Contains("AjaxUrl"))
                        attrUrl = "data-ajax-url";
                    else
                        attrUrl = "data-url";
                    attrBuilder.Add(attrUrl, HtmlCommonUtil.ParseLinkUrl(row, urlContent));
                    string dataConfirm = operRow.ConfirmData;
                    if (!string.IsNullOrEmpty(dataConfirm))
                        attrBuilder.Add("data-confirm", dataConfirm);
                }
                if (info.Contains("_blank"))
                    attrBuilder.Add("data-target", "_blank");
                return string.Format(ObjectUtil.SysCulture, "<li><a {0}>{1}</a></li>",
                    attrBuilder.CreateAttribute(), caption);
            }
        }

        public static string CreateRowOperators(IEnumerable<Operator> operators, IOperatorFieldProvider provider, RowOperatorStyle style)
        {
            if (style == null)
                style = RowOperatorStyle.CreateDefault();

            if (operators == null)
                return string.Empty;
            var rightStr = provider.Operators;
            if (rightStr == null || rightStr.Count == 0)
                return string.Empty;
            HtmlAttributeBuilder attrBuilder = new HtmlAttributeBuilder();
            StringBuilder builder = new StringBuilder();
            string opHtml;
            switch (style.Style)
            {
                case OperatorStyle.Tile:
                case OperatorStyle.Menu:
                    foreach (var operRow in operators)
                    {
                        if (!rightStr.Contains(operRow.Id))
                            continue;
                        builder.Append(CreateRowOperator(provider, attrBuilder, operRow));
                    }
                    opHtml = builder.ToString();
                    if (string.IsNullOrEmpty(opHtml))
                        return string.Empty;
                    string format = style.Style == OperatorStyle.Tile ? Html.TileOperator : Html.MenuOperator;
                    return string.Format(ObjectUtil.SysCulture, format, opHtml, style.MenuCaption);

                case OperatorStyle.Mixture:
                    List<string> tiles = new List<string>();
                    List<string> menus = new List<string>();
                    foreach (var operRow in operators)
                    {
                        string operId = operRow.Id;
                        if (!rightStr.Contains(operId))
                            continue;
                        string oper = CreateRowOperator(provider, attrBuilder, operRow);
                        if (style.IsFix(operId))
                            tiles.Add(oper);
                        else
                            menus.Add(oper);
                    }
                    if (tiles.Count == 0 && menus.Count == 0)
                        return string.Empty;
                    else if (tiles.Count == 0)
                    {
                        //return string.Format(ObjectUtil.SysCulture, Html.MenuOperator, string.Join(string.Empty, menus), style.MenuCaption);
                        if (menus.Count == 1)
                        {
                            return string.Format(ObjectUtil.SysCulture, Html.TileOperator,
                                string.Join(string.Empty, menus));
                        }
                        string menu = menus[0];
                        menus.RemoveAt(0);
                        string totalHtml = string.Format(ObjectUtil.SysCulture, "{0}<li>{1}</li>",
                            string.Join(string.Empty, menu), string.Format(ObjectUtil.SysCulture,
                            Html.MenuOperator, string.Join(string.Empty, menus), style.MenuCaption));
                        return string.Format(ObjectUtil.SysCulture, Html.TileOperator, totalHtml);
                    }
                    else if (menus.Count == 0)
                    {
                        return string.Format(ObjectUtil.SysCulture, Html.TileOperator,
                            string.Join(string.Empty, tiles));
                    }
                    else
                    {
                        string totalHtml = string.Format(ObjectUtil.SysCulture, "{0}<li>{1}</li>",
                            string.Join(string.Empty, tiles), string.Format(ObjectUtil.SysCulture,
                            Html.MenuOperator, string.Join(string.Empty, menus), style.MenuCaption));
                        return string.Format(ObjectUtil.SysCulture, Html.TileOperator, totalHtml);
                    }
                //StringBuilder tileBuilder = new StringBuilder();
                //foreach (var operRow in operators)
                //{
                //    string operId = operRow.Id;
                //    if (!rightStr.Contains(operId))
                //        continue;
                //    string oper = CreateRowOperator(provider, attrBuilder, operRow);
                //    if (style.IsFix(operId))
                //        tileBuilder.Append(oper);
                //    else
                //        builder.Append(oper);
                //}

                //string fixHtml = tileBuilder.ToString();
                //opHtml = builder.ToString();
                //if (string.IsNullOrEmpty(fixHtml) && string.IsNullOrEmpty(opHtml))
                //    return string.Empty;
                //else if (string.IsNullOrEmpty(fixHtml))
                //    return string.Format(ObjectUtil.SysCulture, Html.MenuOperator, opHtml, style.MenuCaption);
                //else if (string.IsNullOrEmpty(opHtml))
                //    return string.Format(ObjectUtil.SysCulture, Html.TileOperator, fixHtml);
                //else
                //{
                //    string totalHtml = string.Format(ObjectUtil.SysCulture, "{0}<li>{1}</li>", fixHtml,
                //        string.Format(ObjectUtil.SysCulture, Html.MenuOperator, opHtml, style.MenuCaption));
                //    return string.Format(ObjectUtil.SysCulture, Html.TileOperator, totalHtml);
                //}
            }
            return string.Empty;
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