using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetMultiEdit", Author = "YJC", CreateDate = "2018-04-20",
        Description = "工作流Content页面模板")]
    internal class WfHisContentPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new WfHisContentPageTemplate();

        private WfHisContentPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return WebRazorUtil.GetTemplateFile("Workflow/contenthistemplate.cshtml");
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return new NormalMultiDetailData();
        }

        public string GetEngineName(ISource source, IInputData input, OutputData outputData)
        {
            return RazorUtil.MULTIEDIT_ENGINE_NAME;
        }

        #endregion IPageTemplate 成员
    }
}