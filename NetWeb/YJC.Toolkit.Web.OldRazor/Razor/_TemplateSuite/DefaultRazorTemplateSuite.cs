using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class DefaultRazorTemplateSuite : BaseNormalTemplateSuite
    {
        public static readonly ITemplateSuite Default = new DefaultRazorTemplateSuite();

        private static readonly string[] fNormalTemplates;
        private static readonly string[] fObjectTemplates;
        private static readonly string fCustomTemplate;

        static DefaultRazorTemplateSuite()
        {
            fCustomTemplate = @"custom\template.cshtml";
            fNormalTemplates = new string[] {
                @"list\template.cshtml",
                @"edit\template.cshtml",
                @"detail\template.cshtml",
                @"detaillist\template.cshtml",
                @"tree\template.cshtml",
                @"tree\detailtemplate.cshtml",
                @"multiedit\template.cshtml",
                @"multidetail\template.cshtml"
            };
            fObjectTemplates = new string[] {
                @"listobject\template.cshtml",
                @"editobject\template.cshtml",
                @"detailobject\template.cshtml",
                null,
                @"treeobject\template.cshtml",
                @"treeobject\detailtemplate.cshtml",
                null,
                null
            };
        }

        private DefaultRazorTemplateSuite()
            : base("BootCss")
        {
        }

        protected override string GetTemplateFile(RazorTemplateStyle style, bool isNormal)
        {
            if (style == RazorTemplateStyle.Custom)
                return fCustomTemplate;
            string[] templates = isNormal ? fNormalTemplates : fObjectTemplates;
            return templates[(int)style];
        }
    }
}
