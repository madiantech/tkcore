using System;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal abstract class BaseCompositeRazorPageMakerConfig : IConfigCreator<IPageMaker>
    {
        protected BaseCompositeRazorPageMakerConfig()
        {
            EditTemplateName = "NormalEdit";
            DetailTemplateName = "NormalDetail";
            ListTemplateName = "NormalList";
        }

        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            IPageData pageData = ObjectUtil.ConfirmQueryObject<IPageData>(this, args);

            PageStyleClass style = PageStyleClass.FromStyle(pageData.Style);
            OverrideItemConfig config = GetConfig(style);
            switch (style.Style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                    if (pageData.IsPost)
                        return CreatePostEditPageMaker(config, style.Style);
                    else
                        return CreateEditPageMaker(config, pageData);

                case PageStyle.Delete:
                    CustomUrlConfig defaultUrl = new CustomUrlConfig(false, false, "ListRefresh");
                    return CreatePostPageMaker(PageStyle.Custom, defaultUrl, config);

                case PageStyle.Detail:
                    return CreateDetailPageMaker(config, pageData);

                case PageStyle.List:
                    return CreateListPageMaker(config, pageData);

                case PageStyle.Custom:
                    return CreateCustomPageMaker(config, pageData, style);
            }

            TkDebug.ThrowImpossibleCode(this);
            return null;
        }

        #endregion IConfigCreator<IPageMaker> 成员

        public string EditTemplateName { get; protected set; }

        public string DetailTemplateName { get; protected set; }

        public string ListTemplateName { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item")]
        public RegNameList<OverrideItemConfig> Items { get; protected set; }

        protected virtual IPageMaker CreateCustomPageMaker(OverrideItemConfig config, IPageData pageData,
            PageStyleClass style)
        {
            if (MetaDataUtil.Equals(style, DbListSource.TabStyle))
                return new JsonObjectPageMaker();

            throw new NotSupportedException();
        }

        protected abstract IPageMaker CreatePostEditPageMaker(OverrideItemConfig config, PageStyle style);

        protected abstract IPageMaker CreateListPageMaker(OverrideItemConfig config, IPageData pageData);

        protected virtual RazorPageMaker CreateEditPageMaker(OverrideItemConfig config, IPageData pageData)
        {
            return CreateRazorPageMaker(EditTemplateName, config, pageData);
        }

        protected virtual RazorPageMaker CreateDetailPageMaker(OverrideItemConfig config, IPageData pageData)
        {
            return CreateRazorPageMaker(DetailTemplateName, config, pageData);
        }

        internal static RazorPageMaker CreateRazorPageMaker(string templateName,
            OverrideItemConfig config, IPageData pageData)
        {
            RazorPageMakerConfig makerConfig = new RazorPageMakerConfig(templateName, config);
            return new RazorPageMaker(makerConfig, pageData);
        }

        protected static PostPageMaker CreatePostPageMaker(PageStyle defaultStyle,
            CustomUrlConfig defaultUrl, OverrideItemConfig config)
        {
            if (config == null)
                return new PostPageMaker(ContentDataType.Json, defaultStyle, defaultUrl);
            else
            {
                if (config.CustomUrl != null)
                    defaultUrl = config.CustomUrl;
                PostPageMaker result = new PostPageMaker(config.DataType, config.DestUrl, defaultUrl);
                if (config.AlertMessage != null)
                    result.AlertMessage = config.AlertMessage.ToString();
                return result;
            }
        }

        protected OverrideItemConfig GetConfig(PageStyleClass style)
        {
            if (Items == null)
                return null;
            return Items[style.ToString()];
        }
    }
}