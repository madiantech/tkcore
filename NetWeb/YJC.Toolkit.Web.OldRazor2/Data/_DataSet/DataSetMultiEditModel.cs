using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class DataSetMultiEditModel : DataSetEditModel, IMultiEditModel
    {
        private IFieldValueProvider fEmptyProvider;

        public DataSetMultiEditModel(DataSet dataSet)
            : base(dataSet)
        {
        }

        #region IMultiEditModel 成员

        public IFieldValueProvider CreateEmptyProvider()
        {
            if (fEmptyProvider == null)
                fEmptyProvider = new DataRowFieldValueProvider(null, DataSet);

            return fEmptyProvider;
        }

        public IEnumerable<IFieldValueProvider> CreateDataRowsProvider(INormalTableData tableData)
        {
            TkDebug.AssertArgumentNull(tableData, "tableData", this);

            return CreateDataRowsProvider(tableData.TableName);
        }

        public IEnumerable<IFieldValueProvider> CreateDataRowsProvider(string tableName)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);

            DataTable table = DataSet.Tables[tableName];
            if (table == null || table.Rows.Count == 0)
                return Enumerable.Empty<IFieldValueProvider>();

            var result = from row in table.AsEnumerable()
                         select new DataRowFieldValueProvider(row, DataSet);
            return result;
        }

        #endregion IMultiEditModel 成员
    }
}