using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2017-04-21",
        Description = "使用Razor引擎，通过预定义的Razor模板，模板对应的PageData以及元数据，再辅以重载Razor文件合成Html输出")]
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