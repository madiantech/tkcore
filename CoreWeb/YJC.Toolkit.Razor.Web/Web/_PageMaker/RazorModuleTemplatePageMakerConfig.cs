using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2017-04-21",
        Description = "整合RazorPageTemplate以及提交的PostPageMaker，以达到整合一个完整功能配置的PageMaker")]
    [ObjectContext]
    internal class RazorModuleTemplatePageMakerConfig : IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            IPageData pageData = ObjectUtil.ConfirmQueryObject<IPageData>(this, args);
            return new RazorModuleTemplatePageMaker(this, pageData);
        }

        #endregion IConfigCreator<IPageMaker> 成员

        [SimpleAttribute(Required = true)]
        public string ModuleTemplate { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "PageTemplate")]
        public List<ModuleOverridePageTemplateConfigItem> PageTemplates { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "PostPageMaker")]
        public List<ModuleOverridePostPageMakerConfigItem> PostPageMakers { get; private set; }
    }
}