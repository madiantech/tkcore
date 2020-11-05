namespace YJC.Toolkit.Sys
{
    public sealed class RedirectorConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_Redirector";
        private const string DESCRIPTION = "页面重定向的配置插件工厂";

        public RedirectorConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }

    }
}
