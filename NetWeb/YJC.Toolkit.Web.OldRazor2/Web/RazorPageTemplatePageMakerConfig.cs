using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ObjectContext]
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2017-04-12",
        Description = "使用Razor引擎，通过预定义的Razor模板，模板对应的PageData以及元数据，再辅以重载Razor文件合成Html输出")]
    internal class RazorPageTemplatePageMakerConfig : BaseRazorPageTemplatePageMakerConfig,
        IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            IPageData pageData = ObjectUtil.ConfirmQueryObject<IPageData>(this, args);
            return new RazorPageTemplatePageMaker(this, pageData);
        }

        #endregion IConfigCreator<IPageMaker> 成员

        [SimpleAttribute(Required = true)]
        public string PageTemplate { get; protected set; }
    }
}