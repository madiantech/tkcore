using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-05-19",
        Description = "使用Razor引擎，通过预定义的Razor模板，模板对应的PageData以及元数据，再辅以重载Razor文件合成Html输出")]
    internal class RazorPageMakerConfig : BaseRazorPageMakerConfig, IConfigCreator<IPageMaker>
    {
        public RazorPageMakerConfig()
        {
        }

        public RazorPageMakerConfig(string template, string razorFile)
        {
            Template = template;
            RazorFile = razorFile;
        }

        public RazorPageMakerConfig(string template, OverrideItemConfig config)
            : this(template, (string)null)
        {
            Assign(config);
        }

        #region IConfigCreator<IPageMaker> 成员

        public virtual IPageMaker CreateObject(params object[] args)
        {
            IPageData pageData = ObjectUtil.ConfirmQueryObject<IPageData>(this, args);

            return new RazorPageMaker(this, pageData);
        }

        #endregion
    }
}
