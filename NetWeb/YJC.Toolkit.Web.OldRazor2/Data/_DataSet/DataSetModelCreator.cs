using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn]
    //[ModelCreator(Author = "YJC", CreateDate = "2017-04-15", Description = "以DataSet的方式创建界面展示所需的数据Model接口")]
    internal class DataSetModelCreator// : BaseDataSetModelCreator
    {
        #region IModelCreator 成员

        public IListModel CreateListModel(object model)
        {
            TkDebug.AssertArgumentNull(model, "model", null);

            DataSet dataSet = model.Convert<DataSet>();
            return new DataSetListModel(dataSet);
        }

        public IEditModel CreateEditModel(object model)
        {
            TkDebug.AssertArgumentNull(model, "model", null);

            DataSet dataSet = model.Convert<DataSet>();
            return new DataSetEditModel(dataSet);
        }

        public IDetailModel CreateDetailModel(object model)
        {
            TkDebug.AssertArgumentNull(model, "model", null);

            DataSet dataSet = model.Convert<DataSet>();
            return new DataSetDetailModel(dataSet);
        }

        #endregion IModelCreator 成员
    }
}