﻿using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ObjectContext]
    internal class PageTemplateConfigItem : BaseXmlPlugInItem
    {
        public const string BASE_CLASS = "_PageTemplate";

        [SimpleAttribute(Required = true)]
        public string TemplateFile { get; private set; }

        [SimpleAttribute(DefaultValue = "Toolkit")]
        public string BaseRazorTemplate { get; private set; }

        [SimpleAttribute]
        public string DefaultModelCreator { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(RazorDataConfigFactory.REG_NAME)]
        public IConfigCreator<object> DefaultPageData { get; private set; }

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }
    }
}