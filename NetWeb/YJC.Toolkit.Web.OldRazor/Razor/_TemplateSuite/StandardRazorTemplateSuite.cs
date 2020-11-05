using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class StandardRazorTemplateSuite : BasePlugInTemplateSuite
    {
        public StandardRazorTemplateSuite(StandardConfig config)
            : base(config.BasePath)
        {
        }

        protected override string GetTemplateFile(RazorTemplateStyle style, bool isNormal)
        {
            throw new NotImplementedException();
        }
    }
}
