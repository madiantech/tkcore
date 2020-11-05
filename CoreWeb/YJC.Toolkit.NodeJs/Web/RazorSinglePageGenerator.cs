using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class RazorSinglePageGenerator : ISinglePageGenerator
    {
        private readonly string fRazorFile;
        private readonly IPageStyle fMetaStyle;
        private readonly IPageStyle fDataStyle;

        public RazorSinglePageGenerator(IEsModel model, string razorTemplate, string subPath,
            string destFile, IPageStyle metaStyle, IPageStyle dataStyle = null)
        {
            TkDebug.AssertArgumentNull(model, nameof(model), null);
            TkDebug.AssertArgumentNullOrEmpty(razorTemplate, nameof(razorTemplate), null);
            TkDebug.AssertArgumentNullOrEmpty(subPath, nameof(subPath), null);
            TkDebug.AssertArgumentNullOrEmpty(destFile, nameof(destFile), null);
            TkDebug.AssertArgumentNull(metaStyle, nameof(metaStyle), null);

            TkDebug.ThrowIfNoAppSetting();

            Model = model;
            DependFile = Path.Combine(EsModelUtil.RazorTemplatePath, razorTemplate, subPath);
            fRazorFile = "^" + Path.Combine(razorTemplate, subPath);
            fMetaStyle = metaStyle;
            fDataStyle = dataStyle ?? PageStyleClass.FromString(fMetaStyle.Style.ToString() + "Vue");
            DestFile = destFile;
        }

        public string DependFile { get; }

        public IEsModel Model { get; }

        public string DestFile { get; }

        public IEnumerable<string> CreateFile(HttpContext context, PageSourceInfo sourceInfo, IModule module)
        {
            ContextWebHandler pageData = new ContextWebHandler(context, sourceInfo, fMetaStyle);
            IMetaData metaData = module.CreateMetaData(pageData);
            ContextWebHandler pageData2 = new ContextWebHandler(context, sourceInfo, fDataStyle);
            TkTrace.LogInfo($"用{fRazorFile}生成文件");
            ISource source = module.CreateSource(pageData2);

            IPrepareSource prepare = source as IPrepareSource;
            if (prepare != null)
                prepare.Prepare(pageData2);
            MetaDataUtil.SetMetaData(source, fDataStyle, metaData);
            OutputData outputData = source.DoAction(pageData2);

            //IPageMaker pageMaker = module.CreatePageMaker(pageData2);
            //MetaDataUtil.SetMetaData(pageMaker, fDataStyle, metaData);
            //var content = pageMaker.WritePage(source, pageData2, outputData);

            string razorContent = RazorHelper.GetRazorResult(outputData, metaData, fRazorFile);
            string destPath = EsModelUtil.GetDestPath(Model, sourceInfo);
            string fileName = Path.Combine(destPath, DestFile);
            TkTrace.LogInfo($"Razor文件输出到{fileName}");
            FileUtil.VerifySaveFile(fileName, razorContent, Encoding.UTF8);

            return FileUtil.GetFileDependecy(metaData);
        }
    }
}