using System;
using System.Collections.Generic;
using System.IO;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class FreeRazorPageMaker : IPageMaker, ISupportMetaData
    {
        private IMetaData fMetaData;
        private readonly string fFileName;
        private readonly Type fBaseType;

        public FreeRazorPageMaker(string fileName)
            : this(fileName, typeof(ToolkitTemplate))
        {
        }

        public FreeRazorPageMaker(string fileName, Type baseType)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);
            TkDebug.AssertArgumentNull(baseType, "baseType", null);
            TkDebug.AssertArgument(File.Exists(fileName), "fileName",
                string.Format(ObjectUtil.SysCulture, "{0}不存在", fileName), null);

            fFileName = fileName;
            fBaseType = baseType;
        }

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return true;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            fMetaData = metaData;
        }

        #endregion

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            object model = WebRazorUtil.GetModel(outputData);
            var viewBag = WebRazorUtil.GetViewBag(pageData, fMetaData, null, RazorData, Assemblies);
            string content = RazorUtil.ParseFromFile(fBaseType, fFileName,
                null, model, viewBag, null);

            return new SimpleContent(ContentTypeConst.HTML, content);
        }

        #endregion

        public IConfigCreator<object> RazorData { get; set; }

        public List<string> Assemblies { get; set; }
    }
}
