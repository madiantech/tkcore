using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [InstancePlugIn]
    [ModelCreator(Author = "YJC", CreateDate = "2017-04-15", Description = "为Edit模板提供包装DataSet的数据模型")]
    internal class DataSetEditModelCreator : BaseDataSetModelCreator
    {
        public static readonly IModelCreator Instance = new DataSetEditModelCreator();

        private DataSetEditModelCreator()
        {
        }

        protected override IModel CreateModelFromDataSet(DataSet dataSet)
        {
            return new DataSetEditModel(dataSet);
        }
    }
}