namespace YJC.Toolkit.Sys
{
    public sealed class ExceptionHanlderConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_ExceptionHanlder";
        private const string DESCRIPTION = "处理Exception的配置插件工厂";

        public ExceptionHanlderConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
