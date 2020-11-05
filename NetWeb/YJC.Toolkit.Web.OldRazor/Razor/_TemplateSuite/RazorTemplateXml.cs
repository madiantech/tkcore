using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class RazorTemplateXml : ToolkitConfig
    {
        [XmlPlugInItem]
        [DynamicElement(RazorTemplateSuiteConfigFactory.REG_NAME)]
        public IConfigCreator<ITemplateSuite> TemplateSuite { get; private set; }
    }
}