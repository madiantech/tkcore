using System;
using System.Collections.Generic;    
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class TemplateItem : BaseTemplateItem
    {
        [SimpleAttribute]
        public string RazorTemplateType { get; private set; }

        [SimpleAttribute]
        public string RazorDataType { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, IsMultiple = true, ObjectType = typeof(string), LocalName = "Assembly")]
        public List<string> AssemblyList { get; private set; }
    }
}