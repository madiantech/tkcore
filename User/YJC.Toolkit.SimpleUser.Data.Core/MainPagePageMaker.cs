using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleRight
{
    [PageMaker(Author = "YJC", CreateDate = "2016-10-10",
        Description = "根据AppSetting的配置切换首页显示的PageMaker")]
    internal class MainPagePageMaker : CompositePageMaker
    {
        public MainPagePageMaker()
        {
            Initialize(null);
        }

        public MainPagePageMaker(IPageData pageData)
            : base(pageData)
        {
            Initialize(pageData);
        }

        private void Initialize(IPageData pageData)
        {
            string style = ""; // ConfigurationManager.AppSettings["mainStyle"];
            bool isAdminlte = string.Compare(style, "AdminLTE",
               StringComparison.CurrentCultureIgnoreCase) == 0;

            Add((source, input, output) => isAdminlte,
                CreatePageMaker("AdminLteMainPage.cshtml", pageData));
            Add((source, input, output) => !isAdminlte,
                CreatePageMaker("MainPage.cshtml", pageData));
        }

        private static IPageMaker CreatePageMaker(string fileName, IPageData pageData)
        {
            string xml = string.Format(ObjectUtil.SysCulture,
                "<tk:FreeRazorPageMaker FileName=\"UserManager/{0}\"/>", fileName);
            var maker = xml.ReadXmlFromFactory<IConfigCreator<IPageMaker>>(
                PageMakerConfigFactory.REG_NAME);
            return maker.CreateObject(pageData);
        }
    }
}