using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Xml;

namespace YJC.Toolkit.Web
{
    public class JsonMetaDataSetPageMaker : IPageMaker, ISupportMetaData
    {
        private IMetaData fMetaData;

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return true;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            fMetaData = metaData;
        }

        #endregion ISupportMetaData 成员

        #region IPageMaker 成员

        public virtual IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            PageMakerUtil.AssertType(source, outputData, SourceOutputType.DataSet);
            DataSet ds = outputData.Data.Convert<DataSet>();
            var metaTables = GetTableData();
            DataSet resultSet = new DataSet(ds.DataSetName) { Locale = ds.Locale };
            using (resultSet)
            {
                foreach (DataTable table in ds.Tables)
                {
                    ITableData tableData;
                    if (metaTables.TryGetValue(table.TableName, out tableData))
                        resultSet.Tables.Add(CreateResultTable(pageData, ds, table, tableData));
                    else
                        resultSet.Tables.Add(table.Copy());
                }
                ProcessResult(resultSet);

                using (XmlReader reader = new XmlDataSetReader(resultSet, true, false))
                {
                    string xml = XmlUtil.GetJson(reader);
                    return new SimpleContent(ContentTypeConst.JSON, xml);
                }
            }
        }

        #endregion IPageMaker 成员

        public IMetaData MetaData
        {
            get
            {
                return fMetaData;
            }
        }

        private static IDisplay GetDisplay(Tk5FieldInfoEx field, IPageData pageData)
        {
            switch (pageData.Style.Style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                    return field.Edit.Display.CreateObject();

                case PageStyle.Detail:
                    return field.ListDetail.DetailDisplay.CreateObject();

                case PageStyle.List:
                    return field.ListDetail.ListDisplay.CreateObject();

                default:
                    if (MetaDataUtil.StartsWith(pageData.Style, "DetailList"))
                        return field.ListDetail.ListDisplay.CreateObject();

                    TkDebug.ThrowToolkitException(
                        string.Format(ObjectUtil.SysCulture, "当前页面类型是{0}，没有可用的Display，请不要使用这种PageMaker",
                        PageStyleClass.FromStyle(pageData.Style)), field);
                    return null;
            }
        }

        private Dictionary<string, ITableData> GetTableData()
        {
            Dictionary<string, ITableData> result = new Dictionary<string, ITableData>();
            if (fMetaData is IListMetaData)
                GetListTableData(result);
            if (fMetaData is INormalMetaData)
                GetNormalTableData(result);

            return result;
        }

        private void GetNormalTableData(Dictionary<string, ITableData> dict)
        {
            var metaData = fMetaData as INormalMetaData;
            if (metaData.Single)
                dict.Add(metaData.TableData.TableName, metaData.TableData);
            else
            {
                foreach (var item in metaData.TableDatas)
                    dict.Add(item.TableName, item);
            }
        }

        private static DataTable CreateResultTable(IPageData pageData, DataSet ds,
            DataTable table, ITableData tableData)
        {
            DataTable resultTable = new DataTable(table.TableName);
            var displayMap = new Dictionary<string, Tuple<Tk5FieldInfoEx, IDisplay>>();
            DataColumnCollection columns = resultTable.Columns;
            foreach (DataColumn col in table.Columns)
                columns.Add(col.ColumnName);

            foreach (var field in tableData.HiddenList)
                displayMap.Add(field.NickName, Tuple.Create(field, GetDisplay(field, pageData)));
            foreach (var field in tableData.DataList)
                displayMap.Add(field.NickName, Tuple.Create(field, GetDisplay(field, pageData)));

            foreach (DataRow row in table.Rows)
            {
                DataRow newRow = resultTable.NewRow();
                DataRowFieldValueProvider provider = new DataRowFieldValueProvider(row, ds);
                newRow.BeginEdit();
                try
                {
                    Tuple<Tk5FieldInfoEx, IDisplay> display;
                    foreach (DataColumn col in table.Columns)
                    {
                        string fieldName = col.ColumnName;
                        if (displayMap.TryGetValue(fieldName, out display))
                            newRow[fieldName] = display.Item2.DisplayValue(provider[fieldName],
                                display.Item1, provider);
                        else
                            newRow[fieldName] = row[col];
                    }
                }
                finally
                {
                    newRow.EndEdit();
                }
                resultTable.Rows.Add(newRow);
            }
            return resultTable;
        }

        private void GetListTableData(Dictionary<string, ITableData> dict)
        {
            var tableData = ((IListMetaData)fMetaData).TableData;
            dict.Add(tableData.TableName, tableData);
        }

        protected virtual void ProcessResult(DataSet resultSet)
        {
        }
    }
}