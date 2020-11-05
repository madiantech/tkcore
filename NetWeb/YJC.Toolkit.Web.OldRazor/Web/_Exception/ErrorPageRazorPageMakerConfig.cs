using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-08-20",
        Description = @"")]
    internal class ErrorPageRazorPageMakerConfig : IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        private static IPageMaker CreatePageMaker()
        {
            string fileName = Path.Combine(BaseAppSetting.Current.XmlPath,
                @"razortemplate\BootCss\Bin\ErrorPage.cshtml");
            return new FreeRazorPageMaker(fileName);
        }
        public IPageMaker CreateObject(params object[] args)
        {
            return CreatePageMaker();
        }

        #endregion
    }
}
