namespace YJC.Toolkit.Sys
{
    public sealed class PostObjectConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_PostObjectCreator";
        private const string DESCRIPTION = "将Post数据转换成对象的配置插件工厂";

        public PostObjectConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
