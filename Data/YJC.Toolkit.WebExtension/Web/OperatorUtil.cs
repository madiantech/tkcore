using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal static class OperatorUtil
    {
        public static string ResolveUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            if (IsAppRelative(url))
                return ToAbsolute(url);
            return url;
        }

        private static string ToAbsolute(string url)
        {
            return url.Substring(1);
        }

        private static bool IsAppRelative(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            return url.StartsWith("~/");
        }

        private static string ParseLinkUrl(IFieldValueProvider provider, string content)
        {
            string linkUrl = ResolveString(provider, content);
            return ResolveUrl(linkUrl);
        }

        private static string ResolveString(IFieldValueProvider provider, string content)
        {
            string linkUrl;
            HRefParser parser = HRefParser.ParseExpression(content);
            object[] dataArray = new object[parser.ParamArray.Count];
            for (int i = 0; i < dataArray.Length; i++)
                dataArray[i] = provider[parser.ParamArray[i]].ToString();
            linkUrl = string.Format(ObjectUtil.SysCulture, parser.FormatString, dataArray);
            return linkUrl;
        }

        public static string CreateRowOperator(IFieldValueProvider row,
            HtmlAttributeBuilder attrBuilder, Operator operRow)
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
                    string js = "javascript:" + ResolveString(row, urlContent);
                    attrBuilder.Add("onClick", js);
                }
                else
                {
                    string attrUrl;
                    if (info.Contains("Dialog"))
                    {
                        string dialogTitle = ResolveString(row, operRow.DialogTitle);
                        attrBuilder.Add("data-title", dialogTitle);
                        attrUrl = "v-tk-dialog";
                    }
                    else if (info.Contains("AjaxUrl"))
                        attrUrl = "v-tk-ajax-url";
                    else
                        attrUrl = "v-tk-push";
                    string oper = urlContent.Contains("?") ? "&" : "?";
                    urlContent = string.Format(ObjectUtil.SysCulture, "'{0}{1}Type=Info'", urlContent, oper);
                    attrBuilder.Add(attrUrl, ParseLinkUrl(row, urlContent));
                    string dataConfirm = operRow.ConfirmData;
                    if (!string.IsNullOrEmpty(dataConfirm))
                        attrBuilder.Add("data-confirm", dataConfirm);
                }
                attrBuilder.Add("class", "tk-btn tk-info");
                if (info.Contains("_blank"))
                    attrBuilder.Add("data-target", "_blank");
                return string.Format(ObjectUtil.SysCulture, "<li><a {0}>{1}</a></li>",
                    attrBuilder.CreateAttribute(), caption);
            }
        }

        public static List<Operator> CreateOperators(DataSet resultSet, string tableName)
        {
            DataTable table = resultSet.Tables[tableName];
            if (table == null)
                return null;
            List<Operator> opers = Operator.ReadFromDataTable(table);
            return opers;
        }
    }
}