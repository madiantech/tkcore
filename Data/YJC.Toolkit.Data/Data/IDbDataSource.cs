using System.Data;

namespace YJC.Toolkit.Data
{
    public interface IDbDataSource
    {
        DataSet DataSet { get; }

        TkDbContext Context { get; }
    }
}
