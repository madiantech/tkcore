using System;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    [DecorateDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "链接显示")]
    [ObjectContext]
    internal class HrefDisplayConfig : IDecorateDisplay,
        IConfigCreator<IDecorateDisplay>
    {
        #region IDecorateDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field,
            IFieldValueProvider rowValue, string linkedValue)
        {
            if (rowValue == null)
                return linkedValue;

            string linkUrl = ResolveRowValue(rowValue, Content);
            if (string.IsNullOrEmpty(linkUrl))
                return linkedValue;
            else
                linkUrl = AppUtil.ResolveUrl(linkUrl);
            string target;

            if (!string.IsNullOrEmpty(Base))
                linkUrl = UriUtil.CombineUri(Base, linkUrl).ToString();
            if (!string.IsNullOrEmpty(Target))
                target = string.Format(ObjectUtil.SysCulture, " target=\"{0}\"", Target);
            else
                target = string.Empty;
            return string.Format(ObjectUtil.SysCulture, "<a href=\"{0}\"{1}>{2}</a>",
                StringUtil.EscapeHtmlAttribute(linkUrl), target, StringUtil.EscapeHtml(linkedValue));
        }

        #endregion IDecorateDisplay 成员

        #region IConfigCreator<IDecorateDisplay> 成员

        public IDecorateDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IDecorateDisplay> 成员

        [TextContent(Required = true)]
        public string Content { get; set; }

        [SimpleAttribute]
        public string Target { get; private set; }

        [SimpleAttribute]
        public string Base { get; private set; }

        //internal static string ResolveObjectString(object receiver, string content)
        //{
        //    string linkUrl;
        //    HRefParser parser = HRefParser.ParseExpression(content);
        //    object[] dataArray = new object[parser.ParamArray.Count];
        //    for (int i = 0; i < dataArray.Length; i++)
        //        dataArray[i] = receiver.MemberValue(parser.ParamArray[i]).ConvertToString();
        //    linkUrl = string.Format(ObjectUtil.SysCulture, parser.FormatString, dataArray);
        //    return linkUrl;
        //}

        //internal static string ResolveString(DataRow row, string content)
        //{
        //    string linkUrl;
        //    HRefParser parser = HRefParser.ParseExpression(content);
        //    object[] dataArray = new object[parser.ParamArray.Count];
        //    for (int i = 0; i < dataArray.Length; i++)
        //        dataArray[i] = row.GetString(parser.ParamArray[i]);
        //    linkUrl = string.Format(ObjectUtil.SysCulture, parser.FormatString, dataArray);
        //    return linkUrl;
        //}

        //internal static string ResolveRowValue(object rowValue, string content, string defaultValue)
        //{
        //    DataRow row = rowValue as DataRow;
        //    if (row != null)
        //    {
        //        return ResolveString(row, content);
        //    }
        //    else
        //    {
        //        ObjectContainer container = rowValue as ObjectContainer;
        //        if (container != null)
        //        {
        //            return ResolveObjectString(container.MainObject, content);
        //        }
        //        else
        //            return defaultValue;
        //    }
        //}

        internal static string ResolveRowValue(IFieldValueProvider provider, string content)
        {
            string linkUrl;
            HRefParser parser = HRefParser.ParseExpression(content);
            object[] dataArray = new object[parser.ParamArray.Count];
            for (int i = 0; i < dataArray.Length; i++)
                dataArray[i] = provider[parser.ParamArray[i]].ToString();
            linkUrl = string.Format(ObjectUtil.SysCulture, parser.FormatString, dataArray);
            return linkUrl;
        }
    }
}