using System.Dynamic;
using System.Threading.Tasks;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class RazorTableOutput : ITableOutput
    {
        private static object Dummy = new object();
        private const string DEFAULT_DLIST_FILENAME = "TableOutput/detaillist.cshtml";

        public RazorTableOutput(string editRazorFile, string detailRazorFile,
            string detailHeadRazorFile = null, string detailListRazorFile = null)
        {
            TkDebug.AssertArgumentNullOrEmpty(editRazorFile, nameof(editRazorFile), null);
            TkDebug.AssertArgumentNullOrEmpty(detailRazorFile, nameof(detailRazorFile), null);

            EditRazorFile = WebRazorUtil.GetTemplateFile(editRazorFile);
            DetailRazorFile = WebRazorUtil.GetTemplateFile(detailRazorFile);
            if (!string.IsNullOrEmpty(detailHeadRazorFile))
                DetailHeadRazorFile = WebRazorUtil.GetTemplateFile(detailHeadRazorFile);
            if (!string.IsNullOrEmpty(detailListRazorFile))
                DetailListRazorFile = WebRazorUtil.GetTemplateFile(detailListRazorFile);
            else
                DetailListRazorFile = WebRazorUtil.GetTemplateFile(DEFAULT_DLIST_FILENAME);
        }

        public virtual bool IsSingle { get => false; }

        public string EditRazorFile { get; }

        public string DetailRazorFile { get; }

        public string DetailHeadRazorFile { get; }

        public string DetailListRazorFile { get; }

        public string CreateEditHtml(INormalTableData tableData, object model, object pageData)
        {
            return RazorOutputFile(tableData, model, pageData, EditRazorFile, true);
        }

        public string CreateDetailHeadHtml(INormalTableData tableData, object pageData)
        {
            if (string.IsNullOrEmpty(DetailHeadRazorFile))
                return string.Empty;

            return RazorOutputFile(tableData, Dummy, pageData, DetailHeadRazorFile, false);
        }

        public string CreateDetailBodyHtml(INormalTableData tableData, object model,
            object pageData, int index)
        {
            return RazorOutputFile(tableData, model, pageData, DetailRazorFile, false, index);
        }

        public string CreateDetailListHtml(IListMetaData tableData, object model, object pageData)
        {
            return RazorOutputFile(tableData, model, pageData, DetailListRazorFile, false);
        }

        protected virtual object CreateCustomData(bool isEdit)
        {
            return null;
        }

        private string RazorOutputFile(object tableData, object model,
            object pageData, string RazorFile, bool isEdit, int? index = null)
        {
            var viewBag = GetNewViewBag(tableData, pageData, CreateCustomData(isEdit), index);
            IRazorEngine engine = RazorEnginePlugInFactory.CreateRazorEngine(RazorUtil.MULTIEDIT_ENGINE_NAME);

            string content = Task.Run(async ()
                => await RazorExtension.CompileRenderWithLayoutAsync(engine, RazorFile,
                null, model, null, viewBag)).GetAwaiter().GetResult();

            return content;
        }

        private static dynamic GetNewViewBag(object metaData, object pageData,
            object customData, int? index)
        {
            dynamic viewBag = new ExpandoObject();
            if (metaData != null)
                viewBag.MetaData = metaData;
            if (pageData != null)
                viewBag.PageData = pageData;
            if (customData != null)
                viewBag.CustomData = customData;
            if (index != null)
                viewBag.Index = index;

            return viewBag;
        }
    }
}