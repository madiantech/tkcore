using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseDataSetModel : IModel
    {
        protected BaseDataSetModel(DataSet dataSet)
        {
            TkDebug.AssertArgumentNull(dataSet, "dataSet", null);

            DataSet = dataSet;
        }

        #region IModel 成员

        public object SourceObject
        {
            get
            {
                return DataSet;
            }
        }

        #endregion IModel 成员

        public DataSet DataSet { get; private set; }
    }
}