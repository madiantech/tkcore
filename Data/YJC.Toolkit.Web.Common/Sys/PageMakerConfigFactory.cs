namespace YJC.Toolkit.Sys
{
    public sealed class PageMakerConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_PageMaker";
        private const string DESCRIPTION = "生成Web页面的配置插件工厂";

        public PageMakerConfigFactory()
            : base(REG_NAME, DESCRIPTION, true)
        {
            DefaultVersion = "1.0";
        }
    }
}