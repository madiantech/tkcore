using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class NotifyActionConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_NotifyAction";
        private const string DESCRIPTION = "NotifyAction配置插件工厂";

        public NotifyActionConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}