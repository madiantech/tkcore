using System.Threading.Tasks;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class FreeRazorPageMaker : IPageMaker, ISupportMetaData
    {
        private IMetaData fMetaData;
        private readonly string fFileName;

        public FreeRazorPageMaker(string fileName)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, nameof(fileName), null);

            fFileName = fileName;
        }

        public IConfigCreator<object> RazorData { get; set; }

        public string Layout { get; set; }

        public string EngineName { get; set; }

        public bool UseTemplate { get; set; }

        public bool CanUseMetaData(IPageStyle style)
        {
            return true;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            fMetaData = metaData;
        }

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            object model = WebRazorUtil.GetModel(outputData);
            var viewBag = WebRazorUtil.GetViewBag(pageData, fMetaData, null, RazorData);
            IRazorEngine engine = RazorEnginePlugInFactory.CreateRazorEngine(EngineName);

            string fileName = UseTemplate ? WebRazorUtil.GetTemplateFile(fFileName) : fFileName;

            string content = Task.Run(async ()
                => await RazorExtension.CompileRenderWithLayoutAsync(engine, fileName,
                Layout, model, null, viewBag)).GetAwaiter().GetResult();

            return new SimpleContent(ContentTypeConst.HTML, content);
        }
    }
}