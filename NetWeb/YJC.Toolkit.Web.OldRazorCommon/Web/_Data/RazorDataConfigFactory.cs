using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class RazorDataConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_RazorData";
        private const string DESCRIPTION = "Razor生成界面的微数据的配置插件工厂";

        public RazorDataConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
