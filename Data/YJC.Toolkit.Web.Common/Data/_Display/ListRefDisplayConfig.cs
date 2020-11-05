using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-26", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "List页面给相应的字段提供详细信息的链接")]
    [ObjectContext]
    internal class ListRefDisplayConfig : IDisplay, IConfigCreator<IDisplay>,
        IHrefDisplay, IDisplayContainer
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            string displayValue = Display.CreateObject().DisplayValue(value, field, rowValue);
            if (string.IsNullOrEmpty(Content))
                return displayValue;

            string linkUrl = HrefDisplayConfig.ResolveRowValue(rowValue, Content);
            if (string.IsNullOrEmpty(linkUrl))
                return displayValue;
            linkUrl = StringUtil.EscapeHtmlAttribute(AppUtil.ResolveUrl(linkUrl));

            switch (Mode)
            {
                case DisplayMode.Normal:
                    return string.Format(ObjectUtil.SysCulture,
                        "<a data-url='{0}' href='#'>{1}</a>", linkUrl, displayValue);

                case DisplayMode.Dialog:
                    string title;
                    if (!string.IsNullOrEmpty(DialogTitle))
                        title = HrefDisplayConfig.ResolveRowValue(rowValue, DialogTitle);
                    else
                        title = displayValue;
                    return string.Format(ObjectUtil.SysCulture,
                        "<a data-dialog-url='{0}' data-title='{2}' href='#'>{1}</a>", linkUrl, displayValue, title);

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return string.Empty;
            }
        }

        #endregion IDisplay 成员

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IDisplay> 成员

        [SimpleAttribute]
        public DisplayMode Mode { get; private set; }

        [SimpleAttribute]
        public string DialogTitle { get; private set; }

        [DynamicElement(CoreDisplayConfigFactory.REG_NAME, Required = true)]
        public IConfigCreator<IDisplay> Display { get; private set; }

        public string Content { get; set; }

        #region IDisplayContainer 成员

        public void SetInternalDisplay(IConfigCreator<IDisplay> display)
        {
            Display = display;
        }

        #endregion IDisplayContainer 成员
    }
}