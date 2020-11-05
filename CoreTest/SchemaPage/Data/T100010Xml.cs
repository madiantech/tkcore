using System;
using YJC.Toolkit.Sys;
using Toolkit.SchemaSuite;

namespace Toolkit.SchemaSuite.Data
{
    public class T100010Xml
    {
        public static readonly QName ROOT = QName.Get("恢复权利请求书");

        [ObjectElement(LocalName = "专利申请或专利", Order = 10)]
        public 专利申请或专利 专利申请或专利 { get; private set; }

        [ObjectElement(LocalName = "请求内容", Order = 20)]
        public 请求内容 请求内容 { get; private set; }

        [ObjectElement(LocalName = "请求恢复权利的理由", Order = 30)]
        public 请求恢复权利的理由 请求恢复权利的理由 { get; private set; }

        [ObjectElement(LocalName = "附件清单", Order = 40)]
        public 附件清单 附件清单 { get; private set; }

        [ObjectElement(LocalName = "申请人或代理机构签章", Order = 50)]
        public 申请人或代理机构签章 申请人或代理机构签章 { get; private set; }

        [ObjectElement(LocalName = "专利局处理意见", Order = 60)]
        public 专利局处理意见 专利局处理意见 { get; private set; }

        public static T100010Xml ReadXmlFromFile(string fileName)
        {
            T100010Xml result = new T100010Xml();
            result.ReadXmlFromFile(fileName, ReadSettings.Default, ROOT);
            return result;
        }
    }
}