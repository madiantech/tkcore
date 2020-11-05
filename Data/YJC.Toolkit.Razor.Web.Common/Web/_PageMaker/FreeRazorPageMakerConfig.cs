using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "使用Razor引擎，但是没有Razor模板，以纯粹的Razor文件生成Html输出",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2019-02-24")]
    internal class FreeRazorPageMakerConfig : IConfigCreator<IPageMaker>
    {
        public IPageMaker CreateObject(params object[] args)
        {
            FreeRazorPageMaker freePageMaker = new FreeRazorPageMaker(FileName)
            {
                Layout = Layout,
                EngineName = EngineName,
                UseTemplate = UseTemplate
            };
            //freePageMaker.RazorData = RazorData;
            return freePageMaker;
        }

        [SimpleAttribute(Required = true)]
        public string FileName { get; private set; }

        [SimpleAttribute]
        public string Layout { get; private set; }

        [SimpleAttribute]
        public string EngineName { get; private set; }

        [SimpleAttribute]
        public bool UseTemplate { get; set; }
    }
}