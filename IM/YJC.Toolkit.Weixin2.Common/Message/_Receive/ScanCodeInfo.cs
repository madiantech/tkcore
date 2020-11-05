using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public class ScanCodeInfo
    {
        [SimpleElement(UseCData = true, Order = 10)]
        public string ScanType { get; private set; }

        [SimpleElement(UseCData = true, Order = 20)]
        public string ScanResult { get; private set; }
    }
}