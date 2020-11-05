using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "基于RazorPageMaker，针对List页面，在QueryString中存在GetData=Page时，只输出列表的数据，而非整个html",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-05-29")]
    internal class ListRazorPageMakerConfig : RazorPageMakerConfig
    {
        private const string DEFAULT_TEMPLATE = "listmain.cshtml";

        public ListRazorPageMakerConfig()
        {
        }

        public ListRazorPageMakerConfig(string template)
            : base(template, (string)null)
        {
            GetDataTemplate = DEFAULT_TEMPLATE;
        }

        public ListRazorPageMakerConfig(string template, OverrideItemConfig config)
            : this(template)
        {
            Assign(config);
        }

        public override IPageMaker CreateObject(params object[] args)
        {
            IPageData pageData = ObjectUtil.ConfirmQueryObject<IPageData>(this, args);

            return new ListRazorPageMaker(this, pageData);
        }

        [SimpleAttribute(DefaultValue = DEFAULT_TEMPLATE)]
        public string GetDataTemplate { get; private set; }
    }
}
