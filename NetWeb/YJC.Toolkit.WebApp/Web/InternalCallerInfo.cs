using System;
using System.Data;
using System.Text;
using System.Xml.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class InternalCallerInfo : ICallerInfo
    {
        private readonly WebPageInfo fPageInfo;

        public InternalCallerInfo(IPageData pageData, SessionGlobal sessionGbl, Uri retUrl, Uri selfUrl)
        {
            fPageInfo = new WebPageInfo(pageData, sessionGbl, retUrl, selfUrl);
        }

        #region ICallerInfo 成员

        public void AddInfo(DataSet dataSet)
        {
            fPageInfo.AddToDataSet(dataSet);
        }

        public void AddInfo(StringBuilder builder)
        {
            fPageInfo.AddToStringBuilder(builder);
        }

        public void AddInfo(XElement element)
        {
            fPageInfo.AddToXElement(element);
        }

        public void AddInfo(dynamic data)
        {
            fPageInfo.AddToDynamic(data);
        }

        #endregion
    }
}
