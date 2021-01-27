using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class DataSetListModel : BaseDataSetModel, IListModel
    {
        private readonly IFieldValueProvider fEmptyProvider;
        private bool fCalTabSheets;
        private List<ListTabSheet> fTabSheets;

        public DataSetListModel(DataSet dataSet)
            : base(dataSet)
        {
            PageStyle = dataSet.GetFieldValue<string>("Info", "Style");
            HasListButtons = CalcHasListButtons(dataSet);
            DecoderSelfUrl = dataSet.GetFieldValue<string>("URL", "DSelfURL");

            DataRow countRow = dataSet.GetRow("Count");
            if (countRow != null)
            {
                PageInfo = new CountInfo(1, 1, 1);
                PageInfo.ReadFromDataRow(countRow);
            }
            DataRow sortRow = dataSet.GetRow("Sort");
            if (sortRow != null)
            {
                SortInfo = new ListSortInfo(null);
                SortInfo.ReadFromDataRow(sortRow);
            }

            DataTable table = dataSet.Tables["ListOperator"];
            ListOperators = Operator.ReadFromDataTable(table);
            table = dataSet.Tables["RowOperator"];
            RowOperators = Operator.ReadFromDataTable(table);

            fEmptyProvider = new DataRowFieldValueProvider(null, dataSet);
        }

        #region IListModel 成员

        public string PageStyle { get; private set; }

        public string Source
        {
            get
            {
                return DataSet.GetFieldValue<string>("Info", "Source");
            }
        }

        public bool HasListButtons { get; private set; }

        public IEnumerable<Operator> ListOperators { get; private set; }

        public CountInfo PageInfo { get; private set; }

        public ListSortInfo SortInfo { get; private set; }

        public string DecoderSelfUrl { get; private set; }

        public IEnumerable<Operator> RowOperators { get; private set; }

        public IEnumerable<ListTabSheet> TabSheets
        {
            get
            {
                if (!fCalTabSheets)
                {
                    DataTable table = DataSet.Tables["TabSheet"];
                    if (table != null && table.Rows.Count > 0)
                    {
                        fTabSheets = table.CreateListFromTable(() => new ListTabSheet(null, null, null));
                    }

                    fCalTabSheets = true;
                }
                return fTabSheets;
            }
        }

        public IEnumerable<IOperatorFieldProvider> CreateDataRowsProvider(IListMetaData metaData)
        {
            DataTable table = DataSet.Tables[metaData.TableData.TableName];
            if (table == null || table.Rows.Count == 0)
                return Enumerable.Empty<IOperatorFieldProvider>();

            var result = from row in table.AsEnumerable()
                         select new DataRowFieldValueProvider(row, DataSet);
            return result;
        }

        public IFieldValueProvider CreateEmptyProvider()
        {
            return fEmptyProvider;
        }

        public IFieldValueProvider CreateQueryProvider()
        {
            DataRow queryRow = DataSet.GetRow("_QueryData");
            IFieldValueProvider queryProvider = new DataRowFieldValueProvider(queryRow, DataSet);
            return queryProvider;
        }

        public IFieldValueProvider CreateProvider(string tableName)
        {
            DataRow row = DataSet.GetRow(tableName);
            IFieldValueProvider provider = new DataRowFieldValueProvider(row, DataSet);
            return provider;
        }

        #endregion IListModel 成员

        private static bool CalcHasListButtons(DataSet dataSet)
        {
            DataTable table = dataSet.Tables["ListOperator"];
            if (table == null)
                return false;
            return table.Rows.Count > 0;
        }
    }
}