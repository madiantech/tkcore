using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class PageTemplateInfo
    {
        public PageTemplateInfo(Func<ISource, IInputData, OutputData, bool> function, string pageTemplate)
        {
            TkDebug.AssertArgumentNull(function, "function", null);
            TkDebug.AssertArgumentNullOrEmpty(pageTemplate, "pageTemplate", null);

            Function = function;
            PageTemplate = pageTemplate;
        }

        public Func<ISource, IInputData, OutputData, bool> Function { get; private set; }

        public string PageTemplate { get; private set; }

        public string ModelCreator { get; set; }
    }
}