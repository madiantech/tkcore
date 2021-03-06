﻿using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetDetail", Author = "YJC", CreateDate = "2017-04-16",
        Description = "MultiDetail页面模板")]
    internal class MultiDetailPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new MultiDetailPageTemplate();

        private MultiDetailPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return WebRazorUtil.GetTemplateFile("MultiDetail/template.cshtml");
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