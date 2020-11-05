using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class UserManagerConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_Workflow_UserManager";
        private const string DESCRIPTION = "工作流用户接口配置插件工厂";

        public UserManagerConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}