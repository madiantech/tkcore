using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    [DecorateDisplayConfig(CreateDate = "2018-04-07", NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        Description = "采用data-url的方式生成链接地址，在链接跳转时，会自动添加RetUrl参数。采用data-dialog-url的方式添加链接，会以对话框形式显示链接地址内容")]
    [ObjectContext]
    internal class DialogHrefDisplayConfig : IDecorateDisplay, IConfigCreator<IDecorateDisplay>
    {
        #region IDecorateDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field,
            IFieldValueProvider rowValue, string linkedValue)
        {
            if (rowValue == null)
                return linkedValue;

            string linkUrl = HrefDisplayConfig.ResolveRowValue(rowValue, Content);
            if (string.IsNullOrEmpty(linkUrl))
                return linkedValue;
            else
                linkUrl = AppUtil.ResolveUrl(linkUrl);
            HtmlAttributeBuilder builder = new HtmlAttributeBuilder();
            if (!string.IsNullOrEmpty(Target))
                builder.Add("target", Target);
            builder.Add("href", "#");
            string urlName = Mode == DisplayMode.Normal ? "data-url" : "data-dialog-url";
            builder.Add(urlName, linkUrl);
            if (Mode == DisplayMode.Dialog && !string.IsNullOrEmpty(DialogTitle))
            {
                string title = HrefDisplayConfig.ResolveRowValue(rowValue, DialogTitle);
                builder.Add("data-title", title);
            }

            return string.Format(ObjectUtil.SysCulture, "<a {0}>{1}</a>", builder.CreateAttribute(),
                StringUtil.EscapeHtml(linkedValue));
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
        public DisplayMode Mode { get; private set; }

        [SimpleAttribute]
        public string DialogTitle { get; private set; }

        [SimpleAttribute]
        public string Target { get; private set; }
    }
}