using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    public class AliyunOSSConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_AliyunOSSConfig";
        private const string DESCRIPTION = "AliyunOSS配置插件工厂";

        public AliyunOSSConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}