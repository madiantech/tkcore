using System.Data;

namespace YJC.Toolkit.Data
{
    internal interface ISqlDataAdapter : IDbDataSource
    {
        IDbDataAdapter DataAdapter { get; }
    }
}
