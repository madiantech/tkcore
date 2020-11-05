using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class DataSetEditModel : BaseDataSetModel, IEditModel
    {
        public DataSetEditModel(DataSet dataSet)
            : base(dataSet)
        {
            PageStyle = DataSet.GetFieldValue<string>("Info", "Style");
            RetUrl = GetRetUrl(dataSet);
        }

        #region IEditModel 成员

        public string PageStyle { get; private set; }

        public string RetUrl { get; private set; }

        public IFieldValueProvider CreateMainObjectProvider(INormalTableData metaData)
        {
            return CreateMainObjectProvider(metaData.TableName);
        }

        public IFieldValueProvider CreateMainObjectProvider(string tableName)
        {
            DataRow row = DataSet.GetRow(tableName);
            return new DataRowFieldValueProvider(row, DataSet);
        }

        #endregion IEditModel 成员

        private static string GetRetUrl(DataSet dataSet)
        {
            string homePage = WebAppSetting.WebCurrent.HomePath;
            if (dataSet == null)
                return homePage;

            string retUrl = dataSet.GetFieldValue<string>("URL", "DRetURL");
            if (string.IsNullOrEmpty(retUrl))
                return homePage;
            return WebUtil.ResolveUrl(retUrl);
        }
    }
}