using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class UploadProcessorConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_Upload";
        private const string DESCRIPTION = "处理上传数据的配置插件工厂";

        public UploadProcessorConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
