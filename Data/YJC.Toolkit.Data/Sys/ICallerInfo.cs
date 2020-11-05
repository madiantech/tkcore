using System.Data;
using System.Text;
using System.Xml.Linq;

namespace YJC.Toolkit.Sys
{
    public interface ICallerInfo
    {
        void AddInfo(DataSet dataSet);

        void AddInfo(StringBuilder builder);

        void AddInfo(XElement element);

        void AddInfo(dynamic data);
    }
}