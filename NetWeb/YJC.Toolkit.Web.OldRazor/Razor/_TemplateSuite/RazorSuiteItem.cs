using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public sealed class RazorSuiteItem
    {
        public RazorSuiteItem(Type razorTemplateType, Type razorDataType, string templateFilePath)
        {
            TkDebug.AssertArgumentNull(razorTemplateType, "razorTemplateType", null);
            TkDebug.AssertArgumentNull(razorDataType, "razorDataType", null);
            TkDebug.AssertArgumentNullOrEmpty(templateFilePath, "templateFilePath", null);

            RazorTemplateType = razorTemplateType;
            RazorDataType = razorDataType;
            TemplateFilePath = templateFilePath;
        }

        public RazorSuiteItem(Type razorTemplateType, Type razorDataType, string templateFilePath, 
            IEnumerable<string> assemblies)
            : this(razorTemplateType, razorDataType, templateFilePath)
        {
            Assemblies = assemblies;
        }

        public Type RazorTemplateType { get; private set; }

        public Type RazorDataType { get; private set; }

        public string TemplateFilePath { get; private set; }

        public IEnumerable<string> Assemblies { get; private set; }
    }
}
