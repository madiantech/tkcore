using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.MetaData
{
    public sealed class Tk5ListTableData : IListTableData
    {
        private readonly ITableOutput fTableOutput;

        private static bool IsShowField(Tk5FieldInfoEx item)
        {
            return item.InternalControl.SrcControl != ControlType.Hidden
                && item.InternalControl.SrcControl != ControlType.None;
        }

        internal Tk5ListTableData(Tk5TableScheme table, ITableOutput tableOutput)
        {
            HiddenList = (from item in table.List
                          where item.InternalControl.SrcControl == ControlType.Hidden
                          select item).ToList();
            TableList = (from item in table.List
                         where IsShowField(item) &&
                            (item.ListDetail == null || item.ListDetail.Search != FieldSearchMethod.Only)
                         orderby item.InternalControl.Order
                         select item).ToList();
            TableName = table.TableName;
            TableDesc = table.TableDesc;
            QueryFields = (from item in table.List
                           where IsShowField(item) &&
                             item.ListDetail != null && item.ListDetail.Search != FieldSearchMethod.False
                           orderby item.InternalControl.Order
                           select item).ToList();
            JsonFieldList fieldList = new JsonFieldList(TableName, QueryFields)
            {
                SearchMethod = SearchControlMethod.Id,
                JsonType = JsonObjectType.Object
            };
            JsonFields = fieldList.ToJsonString();
            NameField = table.NameField;
            fTableOutput = tableOutput;
        }

        #region IListTableData 成员

        IEnumerable<Tk5FieldInfoEx> IListTableData.QueryFields
        {
            get
            {
                return QueryFields;
            }
        }

        ITableOutput IListTableData.Output { get => fTableOutput; }

        #endregion IListTableData 成员

        #region ITableData 成员

        IEnumerable<Tk5FieldInfoEx> ITableData.HiddenList
        {
            get
            {
                return HiddenList;
            }
        }

        IEnumerable<Tk5FieldInfoEx> ITableData.DataList
        {
            get
            {
                return TableList;
            }
        }

        public IFieldInfoEx NameField { get; }

        #endregion ITableData 成员

        [SimpleAttribute]
        public string TableName { get; private set; }

        [SimpleAttribute]
        public string TableDesc { get; private set; }

        [SimpleAttribute]
        public string JsonFields { get; private set; }

        [TagElement(NamespaceType.Toolkit, LocalName = "Hidden")]
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Field")]
        public List<Tk5FieldInfoEx> HiddenList { get; private set; }

        [TagElement(NamespaceType.Toolkit, LocalName = "List")]
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Field")]
        public List<Tk5FieldInfoEx> TableList { get; private set; }

        [TagElement(NamespaceType.Toolkit, LocalName = "Query")]
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Field")]
        public List<Tk5FieldInfoEx> QueryFields { get; private set; }
    }
}