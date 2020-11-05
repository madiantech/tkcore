using System;
using System.Collections.Generic;    
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [RazorTemplateSuiteConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2016-07-17", 
        Author = "YJC", Description = "简单Razor套件模板配置类型，只需要配置对应的模板名称，其他按照默认值处理")]
    internal class SimpleItemsConfig : BaseXmlPlugInItem
    {
        public const string BASE_CLASS = "SimpleItemsTemplateSuite";

        [SimpleAttribute]
        public string BasePath { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "TemplateItem")]
        public List<BaseTemplateItem> TemplateItemList { get; private set; }

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }
    }
}