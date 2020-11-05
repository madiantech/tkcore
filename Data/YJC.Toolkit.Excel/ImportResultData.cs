using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Data.Constraint;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    public sealed class ImportResultData : IDisposable
    {
        internal const string ROW_INDEX = "SrcRowIndex";
        internal const string SESSION_KEY = "_ImportData";
        private readonly Dictionary<Tuple<int, string>, string> fErrors;

        public ImportResultData(Tk5ListMetaData metaData)
        {
            Key = Guid.NewGuid().ToString();
            ImportDataSet = new DataSet(ToolkitConst.TOOLKIT) { Locale = ObjectUtil.SysCulture };
            ImportTable = DataSetUtil.CreateDataTable(metaData.Table.TableName,
                metaData.Table.TableList);
            ImportTable.Columns.Add(ROW_INDEX, typeof(string));
            ImportDataSet.Tables.Add(ImportTable);
            var tableList = (from item in metaData.Table.TableList select item.NickName).ToArray();
            ErrorTable = DataSetUtil.CreateDataTable(metaData.Table.TableName, tableList);
            ErrorTable.Columns.Add(ROW_INDEX, typeof(string));
            fErrors = new Dictionary<Tuple<int, string>, string>();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            ImportDataSet.DisposeObject();
            ErrorTable.DisposeObject();
        }

        #endregion

        public string Key { get; private set; }

        public DataTable ImportTable { get; private set; }

        public DataTable ErrorTable { get; private set; }

        public DataSet ImportDataSet { get; private set; }

        internal Dictionary<Tuple<int, string>, string> Errors
        {
            get
            {
                return fErrors;
            }
        }

        public void AddErrorItem(int rowNumber, string nickName, string errorMsg)
        {
            var key = Tuple.Create(rowNumber, nickName);
            fErrors[key] = errorMsg;
        }

        public string GetErrorMessage(int rowNumber, string nickName)
        {
            var key = Tuple.Create(rowNumber, nickName);
            if (fErrors.ContainsKey(key))
                return fErrors[key];
            return null;
        }

        public void ImportFieldInfoError(FieldErrorInfoCollection errors)
        {
            TkDebug.AssertArgumentNull(errors, "errors", this);

            if (errors.Count == 0)
                return;
            var errorIndexes = from item in errors
                               orderby item.Position descending
                               group item by item.Position;
            foreach (var groupItem in errorIndexes)
            {
                DataRow row = ImportTable.Rows[groupItem.Key];
                int rowNumber = row[ROW_INDEX].Value<int>();
                foreach (var item in groupItem)
                    AddErrorItem(rowNumber, item.NickName, item.Message);
                DataRow errorRow = ErrorTable.NewRow();
                DataSetUtil.CopyRowByName(row, errorRow);
                ErrorTable.Rows.Add(errorRow);
                ImportTable.Rows.Remove(row);
            }
        }

        public void SaveTemp()
        {
            string fileName = FileUtil.GetRealFileName("importTemp.xml", FilePathPosition.Xml);
            ImportDataSet.WriteXml(fileName, XmlWriteMode.WriteSchema);
        }
    }
}
