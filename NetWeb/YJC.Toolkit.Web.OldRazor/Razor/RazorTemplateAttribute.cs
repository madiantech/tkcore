using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class RazorTemplateAttribute : BasePlugInAttribute
    {
        public RazorTemplateAttribute(string templateFile)
        {
            TkDebug.AssertArgumentNullOrEmpty(templateFile, "templateFile", null);

            Suffix = "Template";
            TemplateFile = templateFile;
        }

        public override string FactoryName
        {
            get
            {
                return RazorTemplateTypeFactory.REG_NAME;
            }
        }

        public string TemplateFile { get; private set; }

        public Type PageDataType { get; set; }
    }
}
