using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static partial class BootcssUtil
    {
        //private static string ListButtonFromConfig(Operator optr, object receiver)
        //{
        //    string icon;
        //    if (!string.IsNullOrEmpty(optr.IconClass))
        //        icon = string.Format(ObjectUtil.SysCulture, "<i class=\"{0} mr5\"></i>",
        //            optr.IconClass);
        //    else
        //        icon = string.Empty;
        //    HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
        //    string totalClass = GetTotalClass("mr10", BootcssButton.Default, false);
        //    builder.Add("type", "button");
        //    builder.Add("class", totalClass);

        //    string info = optr.Info;
        //    string attrUrl;
        //    bool isJs = info.Contains("JS");
        //    if (isJs)
        //    {
        //        string js = "javascript:" + HtmlUtil.ResolveObjectString(receiver,
        //            optr.Content);
        //        builder.Add("onClick", js);
        //    }
        //    else
        //    {
        //        if (info.Contains("Dialog"))
        //        {
        //            attrUrl = "data-dialog-url";
        //            string dialogTitle = HtmlUtil.ResolveObjectString(receiver, optr.DialogTitle);
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
        //        builder.Add(attrUrl, HtmlUtil.ParseLinkUrl(receiver, optr.Content));
        //        if (!string.IsNullOrEmpty(optr.ConfirmData))
        //            builder.Add("data-confirm", optr.ConfirmData);
        //    }

        //    string button = string.Format(ObjectUtil.SysCulture, "<button {1}>{2}{0}</button>",
        //        optr.Caption, builder.CreateAttribute(), icon);

        //    return button;
        //}

        //private static string CreateOperators(IEnumerable<Operator> operators, object receiver)
        //{
        //    if (operators == null)
        //        return string.Empty;
        //    StringBuilder builder = new StringBuilder();
        //    foreach (var item in operators)
        //        builder.Append(ListButtonFromConfig(item, receiver));

        //    return builder.ToString();
        //}

        public static string CreateRowOperators(IEnumerable<Operator> operators, IOperatorFieldProvider provider)
        {
            if (operators == null)
                return string.Empty;
            var objectRights = provider.Operators;
            if (objectRights.Count == 0)
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            builder.Append("<ul class=\"operator\">");
            HtmlAttributeBuilder attrBuilder = new HtmlAttributeBuilder();
            foreach (var operRow in operators)
            {
                if (!objectRights.Contains(operRow.Id))
                    continue;
                string caption = operRow.Caption;
                string urlContent = operRow.Content;
                if (string.IsNullOrEmpty(urlContent))
                    builder.AppendFormat(ObjectUtil.SysCulture, "<li>{0}</li>", caption);
                else
                {
                    attrBuilder.Clear();
                    attrBuilder.Add("href", "#");
                    string info = operRow.Info;
                    string attrUrl;
                    if (info.Contains("Dialog"))
                    {
                        string dialogTitle = HtmlCommonUtil.ResolveString(provider, operRow.DialogTitle);
                        attrBuilder.Add("data-title", dialogTitle);
                        attrUrl = "data-dialog-url";
                    }
                    else if (info.Contains("AjaxUrl"))
                        attrUrl = "data-ajax-url";
                    else
                        attrUrl = "data-url";
                    attrBuilder.Add(attrUrl, HtmlCommonUtil.ParseLinkUrl(provider, urlContent));
                    string dataConfirm = operRow.ConfirmData;
                    if (!string.IsNullOrEmpty(dataConfirm))
                        attrBuilder.Add("data-confirm", dataConfirm);
                    builder.AppendFormat(ObjectUtil.SysCulture, "<li><a {0}>{1}</a></li>",
                        attrBuilder.CreateAttribute(), caption);
                }
            }
            builder.Append("</ul>");
            return builder.ToString();
        }

        public static bool HasListButtons(ObjectListModel model)
        {
            return model.ListOperatorCount > 0;
        }

        public static string CreateListButtons(ObjectListModel model)
        {
            IFieldValueProvider provider = new ObjectContainerFieldValueProvider(null, null);
            return BootcssCommonUtil.CreateOperators(model.ListOperators, provider);
        }

        public static string CreateDetailButtons(DetailObjectModel model)
        {
            IFieldValueProvider provider = new ObjectContainerFieldValueProvider(model.Object, null);
            return BootcssCommonUtil.CreateOperators(model.DetailOperators, provider);
        }
    }
}