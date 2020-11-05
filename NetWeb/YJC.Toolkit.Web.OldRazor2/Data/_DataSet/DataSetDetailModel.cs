using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class DataSetDetailModel : DataSetEditModel, IDetailModel
    {
        private readonly List<Operator> fDetailOperators;

        public DataSetDetailModel(DataSet dataSet)
            : base(dataSet)
        {
            DataTable table = dataSet.Tables["DetailOperator"];
            fDetailOperators = Operator.ReadFromDataTable(table);
        }

        #region IDetailModel 成员

        public IEnumerable<Operator> DetailOperators
        {
            get
            {
                return fDetailOperators;
            }
        }

        public string Source
        {
            get
            {
                return DataSet.GetFieldValue<string>("Info", "Source");
            }
        }

        #endregion IDetailModel 成员
    }
}