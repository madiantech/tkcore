using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class UpdatedActionConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_UpdatedAction";
        private const string DESCRIPTION = "UpdatedAction配置插件工厂";

        public UpdatedActionConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}