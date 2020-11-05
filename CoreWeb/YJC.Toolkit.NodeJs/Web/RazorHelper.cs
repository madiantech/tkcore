using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal static class RazorHelper
    {
        private static readonly object Dummy = new object();
        //public static readonly string BasePath = Path.Combine(
        //    BaseAppSetting.Current.SolutionPath, "wwwroot", "htmltest", "vuetest");
        //public static readonly string BaseVuePath = Path.Combine(BasePath, "src", "page");
        //private static readonly Dictionary<PageStyle, string> RazorFiles = new Dictionary<PageStyle, string>
        //{
        //    { PageStyle.Insert, "^WebpackTest/Edit/template.cshtml" },
        //    { PageStyle.Update, "^WebpackTest/Edit/template.cshtml" },
        //    { PageStyle.Detail, "^WebpackTest/Detail/template.cshtml" },
        //    { PageStyle.List, "^WebpackTest/List/template.cshtml" },
        //};
        //private static readonly Dictionary<PageStyle, string> VueFiles = new Dictionary<PageStyle, string>
        //{
        //    { PageStyle.Insert, "components/add.vue" },
        //    { PageStyle.Update, "components/edit.vue" },
        //    { PageStyle.Detail, "components/detail.vue" },
        //    { PageStyle.List, "index.vue" },
        //};

        private static object GetModel(OutputData outputData)
        {
            switch (outputData.OutputType)
            {
                case SourceOutputType.String:
                case SourceOutputType.ToolkitObject:
                case SourceOutputType.DataSet:
                case SourceOutputType.Object:
                    return outputData.Data;

                case SourceOutputType.XmlReader:
                    return XDocument.Load(outputData.Data.Convert<XmlReader>());

                case SourceOutputType.FileContent:
                    TkDebug.ThrowToolkitException("不支持FileContent格式", null);
                    return null;

                default:
                    TkDebug.ThrowImpossibleCode(null);
                    return null;
            }
        }

        private static string Execute(object model, dynamic viewBag, string layout)
        {
            IRazorEngine engine = RazorUtil.ToolkitEngine;

            string content = Task.Run(async ()
                => await RazorExtension.CompileRenderWithLayoutAsync(engine, null,
                layout, model, PageInitData.Instance, viewBag)).GetAwaiter().GetResult();

            return content;
        }

        private static dynamic GetViewBag(IMetaData metaData)
        {
            dynamic result = new ExpandoObject();
            result.MetaData = metaData;

            return result;
        }

        //public static string GetFileContent(IMetaData metaData, string layout)
        //{
        //    dynamic viewBag = GetViewBag(metaData);
        //    return Execute(viewBag, layout);
        //}

        public static string GetRazorResult(OutputData outputData, IMetaData meta, string razorFile)
        {
            dynamic viewBag = GetViewBag(meta);
            object model = GetModel(outputData);
            string result = Execute(model, viewBag, razorFile);
            return result;
        }

        //public static void PrepareFile(PageStyle style, Dictionary<PageStyle, IMetaData> dict)
        //{
        //    string razor = GetRazorResult(style, dict);
        //    string fileName = Path.Combine(BaseVuePath, VueFiles[style]);
        //    FileUtil.VerifySaveFile(fileName, razor, Encoding.UTF8);
        //}
    }
}