using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseDataSetModelCreator : IModelCreator
    {
        protected BaseDataSetModelCreator()
        {
        }

        #region IModelCreator 成员

        public IModel CreateModel(object model)
        {
            DataSet dataSet = ObjectExtension.Convert<DataSet>(model);

            return CreateModelFromDataSet(dataSet);
        }

        #endregion IModelCreator 成员

        protected abstract IModel CreateModelFromDataSet(DataSet dataSet);
    }
}