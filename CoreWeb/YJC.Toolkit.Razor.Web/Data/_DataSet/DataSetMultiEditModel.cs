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

        #endregion IMultiEditModel 成员
    }
}