using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [RazorTemplateSuiteConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2016-07-17",
        Author = "YJC", Description = "简单Razor套件模板配置类型，只需要配置对应的模板名称，其他按照默认值处理")]
    internal class SimpleConfig : BaseXmlPlugInItem
    {
        public const string BASE_CLASS = "SimpleTemplateSuite";
        
        [SimpleAttribute]
        public string BasePath { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public NormalConfigItem Normal { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public NormalConfigItem Object { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public CustomConfigItem Custom { get; private set; }

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }
    }
}