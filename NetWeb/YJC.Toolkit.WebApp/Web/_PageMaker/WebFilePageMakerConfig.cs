using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "将内容用文件的格式输出",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-11")]
    class WebFilePageMakerConfig : IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return WebFilePageMaker.PageMaker;
        }

        #endregion
    }
}
