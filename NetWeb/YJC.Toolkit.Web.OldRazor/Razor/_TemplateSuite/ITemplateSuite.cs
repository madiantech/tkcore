using System;

namespace YJC.Toolkit.Razor
{
    public interface ITemplateSuite
    {
        RazorSuiteItem GetStyleTemplate(
            RazorTemplateStyle style, bool isNormal);
    }
}
