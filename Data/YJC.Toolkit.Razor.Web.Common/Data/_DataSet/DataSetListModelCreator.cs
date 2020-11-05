using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn]
    [ModelCreator(Author = "YJC", CreateDate = "2017-04-15",
        Description = "为List模板提供包装DataSet的数据模型")]
    internal class DataSetListModelCreator : BaseDataSetModelCreator
    {
        public static readonly IModelCreator Instance = new DataSetListModelCreator();

        private DataSetListModelCreator()
        {
        }

        protected override IModel CreateModelFromDataSet(DataSet dataSet)
        {
            return new DataSetListModel(dataSet);
        }
    }
}