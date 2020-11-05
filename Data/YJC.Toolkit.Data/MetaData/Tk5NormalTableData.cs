using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public sealed class Tk5NormalTableData : IRegName, INormalTableData
    {
        public Tk5NormalTableData(Tk5TableScheme table, ISingleMetaData config, SearchControlMethod method, IPageStyle style)
        {
            TkDebug.AssertArgumentNull(table, "table", null);
            TkDebug.AssertArgumentNull(config, "config", null);
            TkDebug.AssertArgumentNull(style, "style", null);

            TableName = table.TableName;
            TableDesc = table.TableDesc;
            NameField = table.NameField;
            ColumnCount = config.ColumnCount;
            HiddenList = (from item in table.List
                          where item.InternalControl.SrcControl == ControlType.Hidden
                          select item).ToList();
            TableList = (from item in table.List
                         where IsShowField(item)
                         orderby item.InternalControl.Order
                         select item).ToList();
            HasEditKey = (from item in TableList where item.IsKey select item).Any();
            Arrange(TableList);
            var editFields = from item in table.List
                             where item.InternalControl.SrcControl != ControlType.None
                             && (config.CommitDetail || item.InternalControl.SrcControl != ControlType.Label)
                             select item;
            JsonFieldList fieldList = new JsonFieldList(TableName, editFields, HasEditKey)
            {
                SearchMethod = method,
                JsonType = config.JsonDataType
            };
            JsonFields = fieldList.ToJsonString();
            Style = PageStyleClass.FromStyle(style);
        }

        internal Tk5NormalTableData(Tk5TableScheme table, BaseSingleMetaDataConfig config, IPageStyle style)
            : this(table, config, SearchControlMethod.Id, style)
        {
        }

        internal Tk5NormalTableData(Tk5TableScheme table, DetailSingleMetaDataConfig config, IPageStyle style)
            : this(table, config.CreateSingleMetaData(), SearchControlMethod.Name, style)
        {
            IsFix = config.IsFix;
            ListStyle = config.ListStyle;
            Output = config.TableOutput?.CreateObject();
        }

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return TableName;
            }
        }

        #endregion IRegName 成员

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

        [SimpleAttribute]
        public int ColumnCount { get; private set; }

        [SimpleAttribute]
        public int RowCount { get; private set; }

        [SimpleAttribute]
        public bool HasEditKey { get; private set; }

        [SimpleAttribute]
        public bool IsFix { get; set; }

        [SimpleAttribute(DefaultValue = TableShowStyle.None)]
        public TableShowStyle ListStyle { get; set; }

        [SimpleAttribute(ObjectType = typeof(PageStyleClass))]
        public IPageStyle Style { get; private set; }

        public ITableOutput Output { get; set; }

        private static bool IsShowField(Tk5FieldInfoEx item)
        {
            return item.InternalControl.SrcControl != ControlType.Hidden
                && item.InternalControl.SrcControl != ControlType.None && item.Layout != null;
        }

        private void Arrange(List<Tk5FieldInfoEx> Fields)
        {
            if (Fields.Count == 0)
                return;

            int currentRow = 0;
            int currentCol = 0;
            foreach (var item in Fields)
            {
                SimpleFieldLayout layout = (SimpleFieldLayout)item.Layout;
                switch (layout.Layout)
                {
                    case FieldLayout.PerUnit:
                        if (layout.UnitNum >= ColumnCount)
                        {
                            layout.Layout = FieldLayout.PerLine;
                            layout.UnitNum = ColumnCount;
                            goto case FieldLayout.PerLine;
                        }
                        if (layout.UnitNum + currentCol <= ColumnCount)
                            currentCol += layout.UnitNum;
                        else
                            ++currentRow;
                        break;

                    case FieldLayout.PerLine:
                        if (currentCol != 0)
                        {
                            currentCol = 0;
                            ++currentRow;
                        }
                        currentRow++;
                        break;
                }
            }
            RowCount = currentRow + 1;
        }
    }
}