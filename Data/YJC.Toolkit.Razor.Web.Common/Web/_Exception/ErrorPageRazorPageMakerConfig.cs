using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-08-20",
        Description = @"使用Normal/Bin/ErrorPage.cshtml输出ErrorPageException的PageMaker(Exception专用)")]
    internal class ErrorPageRazorPageMakerConfig : IConfigCreator<IPageMaker>
    {
        private const string PART_FILE_NAME = "Bin/ErrorPage.cshtml";
        private const string FILE_NAME = @"^Normal/" + PART_FILE_NAME;

        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return CreatePageMaker();
        }

        #endregion IConfigCreator<IPageMaker> 成员

        [SimpleAttribute]
        public bool UseTemplate { get; set; }

        private IPageMaker CreatePageMaker()
        {
            string fileName = UseTemplate ? WebRazorUtil.GetTemplateFile(PART_FILE_NAME) : FILE_NAME;
            return new FreeRazorPageMaker(fileName);
        }
    }
}