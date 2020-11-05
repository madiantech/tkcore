using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WorkflowMetaData : IMetaData, INormalMetaData
    {
        private readonly RegNameList<Tk5TableScheme> fSchemes;

        private WorkflowMetaData()
        {
            fSchemes = new RegNameList<Tk5TableScheme>();
            Tables = new List<Tk5NormalTableData>();
        }

        internal WorkflowMetaData(IInputData input, IEnumerable<TableMetaDataConfig> tables)
            : this()
        {
            if (tables != null)
                foreach (var item in tables)
                    CreateDetailSchemaData(input, item);
        }

        #region IMetaData 成员

        public string Title { get; set; }

        public object ToToolkitObject()
        {
            return this;
        }

        public ITableSchemeEx GetTableScheme(string tableName)
        {
            return fSchemes[tableName];
        }

        #endregion IMetaData 成员

        #region INormalMetaData 成员

        public bool Single
        {
            get
            {
                return Tables.Count == 1;
            }
        }

        public INormalTableData TableData
        {
            get
            {
                if (Tables.Count > 0)
                    return Tables[0];
                return null;
            }
        }

        public IEnumerable<INormalTableData> TableDatas
        {
            get
            {
                return Tables;
            }
        }

        #endregion INormalMetaData 成员

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Table")]
        public List<Tk5NormalTableData> Tables { get; private set; }

        private void CreateDetailSchemaData(IInputData input, TableMetaDataConfig item)
        {
            InputDataProxy proxy = new InputDataProxy(input, (PageStyleClass)item.Action);
            Tk5TableScheme scheme = CreateTableScheme(proxy, item.CreateSingleMetaData());
            Tk5NormalTableData table = new Tk5NormalTableData(scheme, item.CreateSingleMetaData(),
                SearchControlMethod.Name, (PageStyleClass)item.Action);
            if (item.TableOutput != null)
                table.Output = item.TableOutput.CreateObject();
            //else
            //{
            //    if (item.ListStyle == TableShowStyle.Table)
            //        table.Output = new TableOutput { IsFix = item.IsFix };
            //    else
            //        table.Output = new NormalOutput();
            //}
            if (item.ListStyle == TableShowStyle.Table)
                table.ListStyle = TableShowStyle.Table;
            Tables.Add(table);
        }

        private Tk5TableScheme CreateTableScheme(IInputData input, ISingleMetaData item)
        {
            ITableSchemeEx sourceScheme = item.CreateSourceScheme(input);
            Tk5TableScheme scheme = item.CreateTableScheme(sourceScheme, input);
            fSchemes.Add(scheme);
            return scheme;
        }
    }
}