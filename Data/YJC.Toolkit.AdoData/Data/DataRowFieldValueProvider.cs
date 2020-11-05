using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DataRowFieldValueProvider : IFieldValueProvider, IOperatorFieldProvider
    {
        private readonly DataColumnCollection fColumns;

        public DataRowFieldValueProvider(DataRow row, DataSet dataSet)
        {
            TkDebug.AssertArgumentNull(dataSet, "dataSet", null);

            Row = row;
            if (row != null)
                fColumns = row.Table.Columns;
            DataSet = dataSet;
        }

        #region IFieldValueProvider 成员

        public object this[string nickName]
        {
            get
            {
                TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);

                FieldValueProviderName name = nickName.Value<FieldValueProviderName>();
                if (fColumns != null)
                {
                    if (fColumns.Contains(name.NickName))
                        return Row[name.NickName];
                    else if (name.IsDecoder && fColumns.Contains(name.SourceName))
                        return Row[name.SourceName];
                }
                return DBNull.Value;
            }
        }

        public IEnumerable<IDecoderItem> GetCodeTable(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);

            DataTable table = DataSet.Tables[regName];
            if (table == null)
                return null;

            return GetRows(table);
        }

        private IEnumerable<IDecoderItem> GetRows(DataTable table)
        {
            foreach (DataRow row in table.Rows)
                yield return new DataRowDecoderItem(table.Columns, row,
                    DecoderConst.CODE_NICK_NAME, DecoderConst.NAME_NICK_NAME);
        }

        public bool IsEmpty
        {
            get
            {
                return Row == null;
            }
        }

        #endregion IFieldValueProvider 成员

        #region IOperatorFieldProvider 成员

        public ObjectOperatorCollection Operators
        {
            get
            {
                return Row["_OPERATOR_RIGHT"].Value<ObjectOperatorCollection>();
            }
        }

        #endregion IOperatorFieldProvider 成员

        public DataRow Row { get; private set; }

        public DataSet DataSet { get; private set; }
    }
}