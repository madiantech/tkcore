using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetMultiEdit", Author = "YJC", CreateDate = "2017-06-08", Description = "工作流Content页面模板")]
    internal class WfContentPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new WfContentPageTemplate();

        private WfContentPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            if (input.QueryString["Full"] == "content")
                return WebRazorUtil.GetTemplateFile("Workflow/contentfulltemplate.cshtml");
            return WebRazorUtil.GetTemplateFile("Workflow/contenttemplate.cshtml");
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