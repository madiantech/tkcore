using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class TableResolver : TableSelector
    {
        private bool fAutoUpdateKey;
        private readonly static string SetFieldInfoEvent = "SetFieldInfo";
        private readonly static string UpdatingRowEvent = "UpdatingRow";
        private readonly static string UpdatedRowEvent = "UpdatedRow";

        private readonly UpdatingEventArgs fUpdatingArgs = new UpdatingEventArgs();
        private readonly EventHandlerList fEventHandlers = new EventHandlerList();
        private AdapterCommand fCommands;
        private List<IFieldInfo> fListFieldInfos;

        public TableResolver(ITableScheme scheme, IDbDataSource source)
            : base(scheme, source)
        {
        }

        public TableResolver(string tableName, IDbDataSource source)
            : base(tableName, source)
        {
        }

        public TableResolver(string tableName, string keyFields, IDbDataSource source)
            : base(tableName, keyFields, source)
        {
        }

        public TableResolver(string tableName, string keyFields, string fields, IDbDataSource source)
            : base(tableName, keyFields, fields, source)
        {
        }

        protected EventHandlerList EventHandlers
        {
            get
            {
                return fEventHandlers;
            }
        }

        protected bool ReadOnly { get; set; }

        public UpdateMode UpdateMode { get; set; }

        public bool AutoUpdateKey
        {
            get
            {
                return fAutoUpdateKey;
            }
            set
            {
                if (value)
                    TkDebug.Assert(KeyCount == 1,
                        "主键只有一个字段时，AutoUpdateKey属性才可以设置为true", this);
                fAutoUpdateKey = value;
            }
        }

        public bool AutoTrackField { get; set; }

        protected bool MoveDataFlag { get; private set; }

        public virtual List<IFieldInfo> ListFieldInfos
        {
            get
            {
                if (fListFieldInfos == null)
                    fListFieldInfos = (from item in CurrentScheme.Fields
                                       select item).ToList();
                return fListFieldInfos;
            }
        }

        public virtual string GetSqlTableName(TkDbContext context)
        {
            return context.EscapeName(TableName);
        }

        public event EventHandler<FieldInfoEventArgs> SetFieldInfo
        {
            add
            {
                fEventHandlers.AddHandler(SetFieldInfoEvent, value);
            }
            remove
            {
                fEventHandlers.RemoveHandler(SetFieldInfoEvent, value);
            }
        }

        public event EventHandler<UpdatingEventArgs> UpdatingRow
        {
            add
            {
                fEventHandlers.AddHandler(UpdatingRowEvent, value);
            }
            remove
            {
                fEventHandlers.RemoveHandler(UpdatingRowEvent, value);
            }
        }

        public event EventHandler<UpdatingEventArgs> UpdatedRow
        {
            add
            {
                fEventHandlers.AddHandler(UpdatedRowEvent, value);
            }
            remove
            {
                fEventHandlers.RemoveHandler(UpdatedRowEvent, value);
            }
        }

        public void UpdateTrackField(UpdateKind status, DataRow row)
        {
            TkDebug.AssertArgumentNull(row, "row", this);

            // 保证在工作线程中，该代码不会出错。工作线程中UserId要报错
            object userId = DBNull.Value;
            try
            {
                userId = BaseGlobalVariable.UserId;
            }
            catch
            {
            }

            switch (status)
            {
                case UpdateKind.Insert:
                    row["CreateDate"] = row["UpdateDate"] = DateTime.Now;
                    row["CreateId"] = row["UpdateId"] = userId;
                    break;

                case UpdateKind.Update:
                    row["UpdateDate"] = DateTime.Now;
                    row["UpdateId"] = userId;
                    break;
            }
        }

        private void UpdateRow(UpdatingEventArgs e)
        {
            if (AutoUpdateKey)
            {
                if (e.Status == UpdateKind.Insert)
                    e.Row[KeyField] = CreateUniId();
            }
            if (AutoTrackField)
                UpdateTrackField(e.Status, e.Row);

            OnUpdatingRow(e);
            OnUpdatedRow(e);
        }

        private void CopyInsertTable(DataTable postTable, bool isInsert,
            UpdateKind invokeMethod, IInputData inputData)
        {
            SetCommands(AdapterCommand.Insert);
            DataTable dstTable = HostTable;
            if (dstTable == null)
                dstTable = SelectTableStructure();
            DataRow dstRow;
            int i = 0;
            foreach (DataRow srcRow in postTable.Rows)
            {
                bool isInsertRow = false;
                if (!isInsert || dstTable.Rows.Count <= i)
                {
                    dstRow = dstTable.NewRow();
                    isInsertRow = true;
                }
                else
                    dstRow = dstTable.Rows[i];
                CopyRow(srcRow, dstRow, i++);
                if (isInsertRow)
                    dstTable.Rows.Add(dstRow);
                fUpdatingArgs.SetProperties(dstRow, UpdateKind.Insert, invokeMethod, srcRow, null, inputData);
                UpdateRow(fUpdatingArgs);
            }
        }

        private void CopyUpdateTable(DataTable postTable, IInputData inputData)
        {
            SetCommands(AdapterCommand.Update);
            DataTable table = HostTable;

            TkDebug.Assert(table != null && table.Rows.Count == 1, string.Format(ObjectUtil.SysCulture,
                "{0} : 既然选择了OneRow，那么DataSet从数据库取出必须是一条。", TableName), this);
            DataRowCollection srcRows = postTable.Rows;
            TkDebug.Assert(srcRows.Count == 1, string.Format(ObjectUtil.SysCulture,
                "{0} : 既然选择了OneRow，那么你提交的数据也必须是一条。", TableName), this);
            DataRow row = table.Rows[0];
            CopyRow(srcRows[0], row, 0);
            fUpdatingArgs.SetProperties(row, UpdateKind.Update, UpdateKind.Update, srcRows[0], null, inputData);
            UpdateRow(fUpdatingArgs);
        }

        private void CopyDelInsTable(DataTable postTable, DataSet postDataSet, IInputData inputData)
        {
            SetCommands(AdapterCommand.Delete);
            DataTable table = HostTable;
            if (table == null)
                table = SelectTableStructure();
            // Delete the All Row
            foreach (DataRow row in table.Rows)
                DeleteRow(row, UpdateKind.Update, postDataSet, inputData);
            // Copy New DataSet
            if (postTable != null)
                CopyDelInsTable(postTable, UpdateKind.Update, inputData);
        }

        private void CopyDelInsTable(DataTable postTable, UpdateKind invokeMethod, IInputData inputData)
        {
            SetCommands(AdapterCommand.Insert);
            DataTable dstTable = HostTable;
            int i = 0;
            foreach (DataRow srcRow in postTable.Rows)
            {
                DataRow dstRow = dstTable.NewRow();
                CopyRow(srcRow, dstRow, i++);
                fUpdatingArgs.SetProperties(dstRow, UpdateKind.Insert, invokeMethod, srcRow, null, inputData);
                UpdateRow(fUpdatingArgs);
                dstTable.Rows.Add(dstRow);
            }
        }

        /// <summary>
        /// 拷贝合并的数据
        /// </summary>
        /// <param name="postTable">提交的数据表，可以为空</param>
        /// <param name="postDataSet">提交的数据集</param>
        private void CopyMergeTable(DataTable postTable, DataSet postDataSet, IInputData inputData)
        {
            SetCommands(AdapterCommand.All);
            DataTable dstTable = HostTable;
            if (dstTable == null)
                dstTable = SelectTableStructure();

            if (postTable == null)
            {
                foreach (DataRow row in dstTable.Rows)
                    DeleteRow(row, UpdateKind.Update, postDataSet, inputData);
                return;
            }
            SetPrimaryKeys(postTable);
            SetPrimaryKeys(dstTable);

            int i = 0;
            foreach (DataRow row in postTable.Rows)
            {
                UpdateKind status = UpdateKind.Update;
                DataRow dstRow = FindRow(dstTable, row);
                bool isInsert = (dstRow == null);
                if (dstRow == null)
                {
                    dstRow = dstTable.NewRow();
                    status = UpdateKind.Insert;
                }
                CopyRow(row, dstRow, i++);
                fUpdatingArgs.SetProperties(dstRow, status, UpdateKind.Update, row, null, inputData);
                UpdateRow(fUpdatingArgs);
                if (isInsert)
                    dstTable.Rows.Add(dstRow);
            }
            foreach (DataRow row in dstTable.Rows)
            {
                if (row.RowState == DataRowState.Added)
                    continue;
                DataRow srcRow = FindRow(postTable, row);
                if (srcRow == null)
                    DeleteRow(row, UpdateKind.Update, postDataSet, inputData);
            }
        }

        /// <summary>
        /// 把源行的数据拷贝到目的行
        /// </summary>
        /// <param name="srcRow">源行</param>
        /// <param name="dstRow">目的行</param>
        /// <param name="rowPosition">数据行所在的位置</param>
        protected void CopyRow(DataRow srcRow, DataRow dstRow, int rowPosition)
        {
            TkDebug.AssertArgumentNull(srcRow, "srcRow", this);
            TkDebug.AssertArgumentNull(dstRow, "dstRow", this);

            DataColumnCollection dstCols = dstRow.Table.Columns;
            DataColumnCollection srcCols = srcRow.Table.Columns;
            dstRow.BeginEdit();
            try
            {
                foreach (DataColumn dstCol in dstCols)
                {
                    if (dstCol.ReadOnly)
                        continue;
                    if (srcCols.IndexOf(dstCol.ColumnName) != -1)
                        try
                        {
                            object srcValue = srcRow[dstCol.ColumnName];
                            if (string.IsNullOrEmpty(srcValue.ToString()))
                                dstRow[dstCol] = DBNull.Value;
                            else
                                dstRow[dstCol] = srcValue;
                        }
                        catch (Exception)
                        {
                            HandleFormatExeption(Type.GetTypeCode(dstCol.DataType), dstCol.ColumnName, rowPosition);
                        }
                }
            }
            finally
            {
                dstRow.EndEdit();
            }
        }

        /// <summary>
        /// 处理数据类型错误的异常
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="position">所处的数据行的位置</param>
        protected virtual void HandleFormatExeption(TypeCode dataType, string fieldName, int position)
        {
        }

        /// <summary>
        /// 触发更新数据行事件
        /// </summary>
        /// <param name="e">数据更新的参数</param>
        protected virtual void OnUpdatingRow(UpdatingEventArgs e)
        {
            TkDebug.AssertArgumentNull(e, "e", this);

            EventUtil.ExecuteEventHandler(fEventHandlers, UpdatingRowEvent, this, e);
        }

        protected virtual void OnUpdatedRow(UpdatingEventArgs e)
        {
            TkDebug.AssertArgumentNull(e, "e", this);

            EventUtil.ExecuteEventHandler(fEventHandlers, UpdatedRowEvent, this, e);
        }

        /// <summary>
        /// 触发设置字段信息事件
        /// </summary>
        /// <param name="e">字段信息参数</param>
        protected virtual void OnSetFieldInfo(FieldInfoEventArgs e)
        {
            TkDebug.AssertArgumentNull(e, "e", this);

            EventUtil.ExecuteEventHandler(fEventHandlers, SetFieldInfoEvent, this, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fEventHandlers.Dispose();
                DataAdapter.InsertCommand.DisposeObject();
                DataAdapter.UpdateCommand.DisposeObject();
                DataAdapter.DeleteCommand.DisposeObject();
            }

            base.Dispose(disposing);
        }

        public override void ReadMetaData(ITableScheme metaData)
        {
            base.ReadMetaData(metaData);
            SetCommands(fCommands);
        }

        internal List<FieldInfoEventArgs> GetFieldInfo(UpdateKind status)
        {
            List<FieldInfoEventArgs> result = new List<FieldInfoEventArgs>();
            foreach (IFieldInfo fieldInfo in FieldList)
            {
                SqlPosition position = fieldInfo.IsAutoInc ? SqlPosition.None : SqlPosition.Update;
                if (IsKey(fieldInfo.NickName))
                    position |= SqlPosition.Where;
                FieldInfoEventArgs args = new FieldInfoEventArgs(fieldInfo, status, position);
                result.Add(args);
                OnSetFieldInfo(args);
            }
            return result;
        }

        public void SetCommands(AdapterCommand commands)
        {
            if ((int)commands == 0)
                return;

            if ((commands & AdapterCommand.Select) == AdapterCommand.Select)
                SqlBuilder.GetSelectCommand(this);
            if (ReadOnly && commands != AdapterCommand.Select)
                throw new ErrorOperationException("当前是只读状态，无法设置Insert/Update/Delete的Command", this);

            if ((commands & AdapterCommand.Insert) == AdapterCommand.Insert)
                SqlBuilder.GetInsertCommand(this);
            if ((commands & AdapterCommand.Update) == AdapterCommand.Update)
                SqlBuilder.GetUpdateCommand(this);
            if ((commands & AdapterCommand.Delete) == AdapterCommand.Delete)
                SqlBuilder.GetDeleteCommand(this);
            fCommands = commands & (AdapterCommand.Insert | AdapterCommand.Update | AdapterCommand.Delete);
        }

        internal void MergeCommand(TableResolver resolver)
        {
            var commands = fCommands | resolver.fCommands;
            SetCommands(commands);
        }

        internal void InternalUpdateDatabase()
        {
            if (HostTable != null)
            {
                TkTrace.LogInfo(DataAdapter.InsertCommand?.CommandText);
                TkTrace.LogInfo(DataAdapter.UpdateCommand?.CommandText);
                TkTrace.LogInfo(DataAdapter.DeleteCommand?.CommandText);

                (DataAdapter as DbDataAdapter).Update(HostDataSet, TableName);
            }
        }

        public void UpdateDatabase()
        {
            if (ReadOnly)
                throw new ErrorOperationException("当前是只读状态，无法进行数据回写操作", this);

            UpdateUtil.UpdateTableResolvers(Context, null, this);
        }

        public void Insert(DataSet postDataSet) => Insert(postDataSet, null);

        /// <summary>
        /// 在Insert的时候，提交数据
        /// </summary>
        /// <param name="postDataSet">提交的数据集</param>
        public void Insert(DataSet postDataSet, IInputData inputData)
        {
            TkDebug.AssertArgumentNull(postDataSet, "postDataSet", this);

            DataTable postTable = postDataSet.Tables[TableName];
            if (postTable == null)
                return;
            CopyInsertTable(postTable, true, UpdateKind.Insert, inputData);
        }

        public void Update(DataSet postDataSet) => Update(postDataSet, null);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="postDataSet">提交的数据集</param>
        public void Update(DataSet postDataSet, IInputData inputData)
        {
            TkDebug.AssertArgumentNull(postDataSet, "postDataSet", this);

            DataTable postTable = postDataSet.Tables[TableName];
            switch (UpdateMode)
            {
                case UpdateMode.OneRow:
                    if (postTable == null)
                        return;
                    CopyUpdateTable(postTable, inputData);
                    break;

                case UpdateMode.DelIns:
                    if (IsFakeDelete)
                        TkDebug.ThrowToolkitException("DelIns模式下，不支持FakeDelete操作", this);
                    CopyDelInsTable(postTable, postDataSet, inputData);
                    break;

                case UpdateMode.Merge:
                    if (IsFakeDelete)
                        TkDebug.ThrowToolkitException("Merge模式下，不支持FakeDelete操作", this);
                    CopyMergeTable(postTable, postDataSet, inputData);
                    break;
            }
        }

        public void Delete(bool moveData = false) => Delete(null, moveData);

        /// <summary>
        /// 删除数据
        /// </summary>
        public void Delete(IInputData inputData, bool moveData = false)
        {
            DataTable table = HostTable;
            if (table == null)
                return;

            MoveDataFlag = moveData;
            if (IsFakeDelete)
            {
                SetCommands(AdapterCommand.Update);

                for (int i = table.Rows.Count - 1; i >= 0; --i)
                {
                    DataRow row = table.Rows[i];
                    FakeDeleteRow(row);
                    InternalFakeDeleteRow(row, inputData);
                }
            }
            else
            {
                SetCommands(AdapterCommand.Delete);
                for (int i = table.Rows.Count - 1; i >= 0; --i)
                {
                    DataRow row = table.Rows[i];
                    DeleteRow(row, UpdateKind.Delete, null, inputData);
                }
            }
        }

        public void SqlDelete(IParamBuilder builder)
        {
            TkDebug.AssertArgumentNull(builder, "builder", this);

            if (IsFakeDelete)
            {
                var field = GetFieldInfo(FakeDelete.FieldName);
                string sql = string.Format(ObjectUtil.SysCulture, "UPDATE {0} SET {1} = {2}{3}",
                    TableName, field.FieldName, Context.GetSqlParamName(field.FieldName),
                    string.IsNullOrEmpty(builder.Sql) ? string.Empty : " WHERE " + builder.Sql);
                DbParameterList paramList = new DbParameterList();
                paramList.Add(field, FakeDelete.Value);
                paramList.Add(builder.Parameters);
                DbUtil.ExecuteNonQuery(sql, Context, paramList);
            }
            else
            {
                string sql = "DELETE FROM " + TableName;
                DbUtil.ExecuteNonQuery(sql, Context, builder);
            }
        }

        protected virtual void FakeDeleteRow(DataRow row)
        {
            row[FakeDelete.FieldName] = FakeDelete.Value;
        }

        private void InternalFakeDeleteRow(DataRow row, IInputData inputData)
        {
            fUpdatingArgs.SetProperties(row, UpdateKind.Update, UpdateKind.Delete,
                null, null, inputData);
            UpdateRow(fUpdatingArgs);
        }

        internal void DeleteRow(DataRow row, UpdateKind invokeMethod,
            DataSet postDataSet, IInputData inputData)
        {
            fUpdatingArgs.SetProperties(row, UpdateKind.Delete, invokeMethod, null, postDataSet, inputData);
            UpdateRow(fUpdatingArgs);
            row.Delete();
        }

        public virtual void AddVirtualFields()
        {
        }

        public DataRow Query(IQueryString queryString)
        {
            TkDebug.AssertArgumentNull(queryString, "queryString", this);

            if (KeyCount == 1)
                return SelectRowWithKeys(queryString[Keys[0].NickName]);
            else
            {
                object[] keyValues = new object[KeyCount];
                for (int i = 0; i < KeyCount; ++i)
                    keyValues[i] = queryString[Keys[i].NickName];
                return SelectRowWithKeys(keyValues);
            }
        }

        public void Query(DataSet postDataSet)
        {
            TkDebug.AssertArgumentNull(postDataSet, "postDataSet", this);

            DataTable table = postDataSet.Tables[TableName];
            if (table == null)
            {
                SelectTableStructure();
                return;
            }

            TkDebug.Assert(KeyCount > 0, string.Format(ObjectUtil.SysCulture,
                "没有给表{0}配置主键，请检查", TableName), this);
            bool hasOldColumn = table.Columns.Contains("OLD_" + Keys[0].NickName);
            foreach (DataRow srcRow in table.Rows)
            {
                if (KeyCount == 1)
                {
                    if (hasOldColumn)
                        SelectWithKeys(srcRow["OLD_" + Keys[0].NickName]);
                    else
                        SelectWithKeys(srcRow[Keys[0].NickName]);
                }
                else
                {
                    object[] fieldValues = new string[KeyCount];
                    for (int i = 0; i < KeyCount; ++i)
                    {
                        if (hasOldColumn)
                            fieldValues[i] = srcRow["OLD_" + Keys[i].NickName];
                        else
                            fieldValues[i] = srcRow[Keys[i].NickName];
                    }
                    SelectWithKeys(fieldValues);
                }
            }
        }

        public virtual KeyData CreateKeyData(DataRow row)
        {
            if (KeyCount == 1)
            {
                IFieldInfo key = Keys[0];
                return new KeyData(key.NickName, row[key.NickName].ToString());
            }
            else
            {
                var keyPairs = from key in Keys
                               select new KeyValuePair<string, string>(
                                   key.NickName, row[key.NickName].ToString());
                return new KeyData(keyPairs);
            }
        }

        internal void SetDefaultValue(IQueryString queryString)
        {
            DataTable table = HostTable;
            if (table == null)
                return;

            foreach (DataRow row in table.Rows)
                foreach (DataColumn col in table.Columns)
                {
                    string value = queryString[col.ColumnName];
                    if (!string.IsNullOrEmpty(value))
                        DataSetUtil.SetSafeValue(row, col, value);
                }
        }

        public KeyData CreateKeyData()
        {
            DataTable table = HostTable;
            TkDebug.Assert(table.Rows.Count > 0, "当前表中的记录不能为0", this);

            return CreateKeyData(table.Rows[0]);
        }

        public void PrepareDataSet(DataSet dataSet)
        {
            TkDebug.AssertArgumentNull(dataSet, "dataSet", this);

            DataTable table = dataSet.Tables[TableName];
            if (table != null)
            {
                for (int i = table.Rows.Count - 1; i >= 0; --i)
                {
                    DataRow row = table.Rows[i];

                    // 删除空行
                    bool isEmpty = true;
                    for (int j = 0; j < table.Columns.Count; ++j)
                        if (!string.IsNullOrEmpty(row[j].ToString()))
                        {
                            isEmpty = false;
                            break;
                        }
                    if (isEmpty)
                    {
                        table.Rows.RemoveAt(i);
                        continue;
                    }

                    // 新增行设置负数为主键
                    foreach (IFieldInfo field in Keys)
                        if (string.IsNullOrEmpty(row[field.NickName].ToString()))
                            row[field.NickName] = -i;
                }
            }
        }

        public virtual void SetDefaultValue(DataRow row)
        {
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "表{0}的数据读写器", TableName);
        }
    }
}