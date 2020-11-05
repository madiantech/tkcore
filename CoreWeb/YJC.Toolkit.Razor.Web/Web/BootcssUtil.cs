using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static class BootcssUtil
    {
        private static string ListButtonFromConfig(Operator optr, IFieldValueProvider receiver)
        {
            string icon;
            if (!string.IsNullOrEmpty(optr.IconClass))
                icon = string.Format(ObjectUtil.SysCulture, "<i class=\"{0} mr5\"></i>",
                    optr.IconClass);
            else
                icon = string.Empty;
            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            string totalClass = BootcssCommonUtil.GetTotalClass("mr10", BootcssButton.Default, false);
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
                builder.Add(attrUrl, HtmlUtil.ParseLinkUrl(receiver, optr.Content));
                if (!string.IsNullOrEmpty(optr.ConfirmData))
                    builder.Add("data-confirm", optr.ConfirmData);
            }

            string button = string.Format(ObjectUtil.SysCulture, "<button {1}>{2}{0}</button>",
                optr.Caption, builder.CreateAttribute(), icon);

            return button;
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
                    attrBuilder.Add(attrUrl, HtmlUtil.ParseLinkUrl(row, urlContent));
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

        public static string CreateOperators(IEnumerable<Operator> operators, IFieldValueProvider receiver)
        {
            if (operators == null)
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            foreach (var item in operators)
                builder.Append(ListButtonFromConfig(item, receiver));

            return builder.ToString();
        }
    }
}