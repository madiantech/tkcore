using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class ListRazorPageMaker : RazorPageMaker
    {
        private readonly ListRazorPageMakerConfig fConfig;

        public ListRazorPageMaker(ListRazorPageMakerConfig config, IPageData pageData)
            : base(config, pageData)
        {
            fConfig = config;
        }

        protected override string CreateRazorContent(IPageData pageData, object model,
            DynamicObjectBag viewBag)
        {
            if (pageData.QueryString["GetData"] == "Page" || pageData.IsPost)
                return RazorTemplateUtil.Execute(fConfig.Template, fConfig.GetDataTemplate,
                    fConfig.RazorFile, model, viewBag);
            return base.CreateRazorContent(pageData, model, viewBag);
        }
    }
}
