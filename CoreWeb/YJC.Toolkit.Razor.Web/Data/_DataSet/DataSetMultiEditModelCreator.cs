using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn]
    [ModelCreator(Author = "YJC", CreateDate = "2017-04-18",
        Description = "为MultiEdit模板提供包装DataSet的数据模型")]
    internal class DataSetMultiEditModelCreator : BaseDataSetModelCreator
    {
        public static readonly IModelCreator Instance = new DataSetMultiEditModelCreator();

        private DataSetMultiEditModelCreator()
        {
        }

        protected override IModel CreateModelFromDataSet(DataSet dataSet)
        {
            return new DataSetMultiEditModel(dataSet);
        }
    }
}