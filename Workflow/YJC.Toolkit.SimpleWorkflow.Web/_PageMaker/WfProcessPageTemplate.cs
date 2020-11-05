using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetMultiEdit", Author = "YJC", CreateDate = "2017-06-08", Description = "工作流Process页面模板")]
    internal class WfProcessPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new WfProcessPageTemplate();

        private WfProcessPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return WebRazorUtil.GetTemplateFile("Workflow/processtemplate.cshtml");
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return new NormalMultiEditData();
        }

        public string GetEngineName(ISource source, IInputData input, OutputData outputData)
        {
            return RazorUtil.MULTIEDIT_ENGINE_NAME;
        }

        #endregion IPageTemplate 成员
    }
}