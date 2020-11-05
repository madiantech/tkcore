using System.Collections.Generic;
using System.Data;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static partial class BootcssUtil
    {
        //private static string ListButtonFromConfig(DataRow operatorRow, DataRow dataRow)
        //{
        //    string icon;
        //    string iconClass = operatorRow.GetString("IconClass");
        //    if (!string.IsNullOrEmpty(iconClass))
        //        icon = string.Format(ObjectUtil.SysCulture, "<i class=\"{0} mr5\"></i>", iconClass);
        //    else
        //        icon = string.Empty;
        //    HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
        //    string totalClass = GetTotalClass("mr10", BootcssButton.Default, false);
        //    builder.Add("type", "button");
        //    builder.Add("class", totalClass);

        //    string caption = operatorRow.GetString("Caption");
        //    string info = operatorRow.GetString("Info");
        //    string attrUrl;
        //    bool isJs = info.Contains("JS");
        //    if (isJs)
        //    {
        //        string js = "javascript:" + HtmlUtil.ResolveString(dataRow,
        //            operatorRow.GetString("Content"));
        //        builder.Add("onClick", js);
        //    }
        //    else
        //    {
        //        if (info.Contains("Dialog"))
        //        {
        //            attrUrl = "data-dialog-url";
        //            string dialogTitle = HtmlUtil.ResolveString(dataRow, operatorRow.GetString("DialogTitle"));
        //            builder.Add("data-title", dialogTitle);
        //        }
        //        else if (info.Contains("AjaxUrl"))
        //        {
        //            attrUrl = "data-ajax-url";
        //            if (info.Contains("Post"))
        //                builder.Add("data-method", "post");
        //        }
        //        else
        //            attrUrl = "data-url";
        //        builder.Add(attrUrl, HtmlUtil.ParseLinkUrl(dataRow, operatorRow.GetString("Content")));
        //        string dataConfirm = operatorRow.GetString("ConfirmData");
        //        if (!string.IsNullOrEmpty(dataConfirm))
        //            builder.Add("data-confirm", dataConfirm);
        //    }

        //    string button = string.Format(ObjectUtil.SysCulture, "<button {1}>{2}{0}</button>",
        //        caption, builder.CreateAttribute(), icon);

        //    return button;
        //}

        private static string CreateOperators(DataTable table, DataRow dataRow)
        {
            if (table == null)
                return string.Empty;
            //StringBuilder builder = new StringBuilder();
            List<Operator> opers = Operator.ReadFromDataTable(table);
            IFieldValueProvider provider = new DataRowFieldValueProvider(dataRow, table.DataSet);

            //foreach (DataRow row in table.Rows)
            //    builder.Append(ListButtonFromConfig(row, dataRow));

            //return builder.ToString();
            return BootcssCommonUtil.CreateOperators(opers, provider);
        }

        public static bool HasListButtons(DataSet dataSet)
        {
            DataTable table = dataSet.Tables["ListOperator"];
            if (table == null)
                return false;
            return table.Rows.Count > 0;
        }

        public static bool HasTabSheet(DataSet dataSet)
        {
            DataTable table = dataSet.Tables["TabSheet"];
            if (table == null)
                return false;
            return table.Rows.Count > 0;
        }

        public static string CreateTabSheets(DataSet dataSet)
        {
            DataTable table = dataSet.Tables["TabSheet"];
            if (table == null)
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            foreach (DataRow row in table.Rows)
            {
                string active = string.Empty;
                if (row["Selected"].Value<bool>())
                    active = " class=\"active\"";
                builder.AppendFormat(ObjectUtil.SysCulture,
                    "<li role=\"presentation\"{0}><a href=\"#\" data-tab=\"{1}\">{2}</a></li>\n",
                    active, row["Id"], row["Caption"]);
            }
            return builder.ToString();
        }

        public static string CreateListButtons(DataSet dataSet)
        {
            DataTable table = dataSet.Tables["ListOperator"];
            List<Operator> opers = Operator.ReadFromDataTable(table);
            IFieldValueProvider provider = new DataRowFieldValueProvider(null, dataSet);
            return BootcssCommonUtil.CreateOperators(opers, provider);
        }

        public static string CreateDetailButtons(DataSet dataSet, DataRow row)
        {
            DataTable table = dataSet.Tables["DetailOperator"];
            return CreateOperators(table, row);
        }

        private static string CreateRowOperator(DataRow row, HtmlAttributeBuilder attrBuilder, DataRow operRow)
        {
            string caption = operRow.GetString("Caption");
            string urlContent = operRow.GetString("Content");
            if (string.IsNullOrEmpty(urlContent))
                return string.Format(ObjectUtil.SysCulture, "<li>{0}</li>", caption);
            else
            {
                attrBuilder.Clear();
                attrBuilder.Add("href", "#");
                string info = operRow.GetString("Info");
                bool isJs = info.Contains("JS");
                if (isJs)
                {
                    string js = "javascript:" + HtmlUtil.ResolveString(row, urlContent);
                    attrBuilder.Add("onClick", js);
                }
                else
                {
                    string attrUrl;
                    if (info.Contains("Dialog"))
                    {
                        string dialogTitle = HtmlUtil.ResolveString(row, operRow.GetString("DialogTitle"));
                        attrBuilder.Add("data-title", dialogTitle);
                        attrUrl = "data-dialog-url";
                    }
                    else if (info.Contains("AjaxUrl"))
                        attrUrl = "data-ajax-url";
                    else
                        attrUrl = "data-url";
                    attrBuilder.Add(attrUrl, HtmlUtil.ParseLinkUrl(row, urlContent));
                    string dataConfirm = operRow.GetString("ConfirmData");
                    if (!string.IsNullOrEmpty(dataConfirm))
                        attrBuilder.Add("data-confirm", dataConfirm);
                }
                if (info.Contains("_blank"))
                    attrBuilder.Add("data-target", "_blank");
                return string.Format(ObjectUtil.SysCulture, "<li><a {0}>{1}</a></li>",
                    attrBuilder.CreateAttribute(), caption);
            }
        }

        public static string CreateRowOperators(DataSet dataSet, DataRow row)
        {
            return CreateRowOperators(dataSet, row, null);
        }

        public static string CreateRowOperators(DataSet dataSet, DataRow row, RowOperatorStyle style)
        {
            if (style == null)
                style = RowOperatorStyle.CreateDefault();

            DataTable table = dataSet.Tables["RowOperator"];
            if (table == null)
                return string.Empty;
            List<Operator> opers = Operator.ReadFromDataTable(table);
            DataRowFieldValueProvider provider = new DataRowFieldValueProvider(row, dataSet);
            return BootcssCommonUtil.CreateRowOperators(opers, provider, style);
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
    }
}