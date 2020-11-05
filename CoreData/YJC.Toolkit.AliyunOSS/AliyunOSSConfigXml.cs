using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    internal class AliyunOSSConfigXml : ToolkitConfig
    {
        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public AliyunOSSConfig AliyunOSS { get; private set; }
    }
}