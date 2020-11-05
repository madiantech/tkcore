using System.Data;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    public interface IConnection
    {
        bool Match(DataRow mainRow, IDbDataSource source);
    }
}