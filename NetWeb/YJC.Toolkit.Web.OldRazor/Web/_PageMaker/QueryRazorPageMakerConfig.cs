using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "Get时使用条件模板输出条件，Post时使用结果模板输出结果。是针对Query模型的PageMaker",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-10-30")]
    internal class QueryRazorPageMakerConfig : RazorPageMakerConfig
    {
        [SimpleAttribute(DefaultValue = "NormalQueryCondition")]
        public string ConditionTemplate { get; private set; }

        [SimpleAttribute(DefaultValue = "NormalQueryResult")]
        public string ResultTemplate { get; private set; }

        [SimpleAttribute]
        public string ConditionRazorFile { get; private set; }

        [SimpleAttribute]
        public string ResultRazorFile { get; private set; }

        public override IPageMaker CreateObject(params object[] args)
        {
            IPageData pageData = ObjectUtil.ConfirmQueryObject<IPageData>(this, args);
            if (pageData.IsPost)
            {
                RazorFile = ResultRazorFile;
                Template = ResultTemplate;
            }
            else
            {
                RazorFile = ConditionRazorFile;
                Template = ConditionTemplate;
            }

            return new RazorPageMaker(this, pageData);
        }

    }
}
