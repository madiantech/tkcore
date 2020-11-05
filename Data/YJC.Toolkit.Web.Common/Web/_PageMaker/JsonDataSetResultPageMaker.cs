using System.Collections.Generic;
using System.Data;
using System.Xml;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Xml;

namespace YJC.Toolkit.Web
{
    internal class JsonDataSetResultPageMaker : IPageMaker
    {
        private readonly List<string> fRemoveTables;
        private readonly Dictionary<string, List<string>> fTableMappings;

        public JsonDataSetResultPageMaker()
        {
            fRemoveTables = new List<string>();
            fTableMappings = new Dictionary<string, List<string>>();
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            TkDebug.AssertArgumentNull(outputData, "outputData", this);

            PageMakerUtil.AssertType(source, outputData,
                SourceOutputType.XmlReader, SourceOutputType.DataSet);

            if (outputData.OutputType == SourceOutputType.DataSet)
            {
                DataSet ds = PageMakerUtil.GetObject<DataSet>(outputData);
                DataTableCollection tables = ds.Tables;
                foreach (string tableName in fRemoveTables)
                {
                    if (tables.Contains(tableName))
                        tables.Remove(tableName);
                }
                foreach (var mapping in fTableMappings)
                {
                    if (tables.Contains(mapping.Key))
                    {
                        DataTable table = tables[mapping.Key];
                        DataColumnCollection columns = table.Columns;
                        foreach (string field in mapping.Value)
                        {
                            if (columns.Contains(field))
                                columns.Remove(field);
                        }
                    }
                }
            }

            using (XmlReader reader = PageMakerUtil.GetDataSetReader(outputData))
            {
                string xml = XmlUtil.GetJson(reader, Result);
                return new SimpleContent(ContentTypeConst.JSON, xml);
            }
        }

        #endregion

        public ActionResultData Result { get; set; }

        public void AddRemoveTable(string tableName)
        {
            fRemoveTables.Add(tableName);
        }

        public bool RemoveRemoveTable(string tableName)
        {
            return fRemoveTables.Remove(tableName);
        }

        public void AddRemoveField(string tableName, string fieldName)
        {
            List<string> fields;
            if (fTableMappings.TryGetValue(tableName, out fields))
                fields.Add(fieldName);
            else
            {
                fields = new List<string> { fieldName };
                fTableMappings.Add(tableName, fields);
            }
        }

        public bool RemoveRemoveField(string tableName, string fieldName)
        {
            List<string> fields;
            if (fTableMappings.TryGetValue(tableName, out fields))
                return fields.Remove(fieldName);

            return false;
        }
    }
}
