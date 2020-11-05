using System;
using System.Collections.Generic;    
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [RazorTemplateSuiteConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2016-07-17",
        Author = "YJC", Description = "标准Razor套件模板注册类型")]
    internal class StandardConfig : BaseXmlPlugInItem
    {
        public const string BASE_CLASS = "StandardTemplateSuite";
        
        [SimpleAttribute]
        public string BasePath { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, ObjectType = typeof(TemplateItem), LocalName = "TemplateItem")]
        public List<TemplateItem> TemplateItemList { get; private set; }

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }
    }
}