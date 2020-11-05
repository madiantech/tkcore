using System.Data;

namespace YJC.Toolkit.Data
{
    public interface ISimpleAdapter
    {
        string SelectSql { get; }

        void SetSql(string sql, IParamBuilder builder);

        int Fill(DataSet dataSet, string srcTable);

        int Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable);
    }
}
