using System;
using System.Collections.Generic;
using System.Transactions;
using YJC.Toolkit.Log;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class MultipleDbDelInsSource : BaseMultipleDbSource, ICommitEvent,
        INonUIResolvers, ISupportRecordLog
    {
        private TableResolverCollection fNonUIResolvers;
        private Dictionary<string, Tuple<IRecordDataPicker, List<RecordLogData>>> fRecordLogs;

        public override OutputData DoAction(IInputData input)
        {
            throw new NotImplementedException();
        }

        #region ICommitEvent 成员

        public event EventHandler<CommittingDataEventArgs> CommittingData
        {
            add
            {
                EventHandlers.AddHandler(EventConst.CommittingDataEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.CommittingDataEvent, value);
            }
        }

        public event EventHandler<CommittedDataEventArgs> CommittedData
        {
            add
            {
                EventHandlers.AddHandler(EventConst.CommittedDataEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.CommittedDataEvent, value);
            }
        }

        public event EventHandler<ApplyDataEventArgs> ApplyData
        {
            add
            {
                EventHandlers.AddHandler(EventConst.ApplyDataEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.ApplyDataEvent, value);
            }
        }

        #endregion ICommitEvent 成员

        #region INonUIResolvers 成员

        public TableResolverCollection NonUIResolvers
        {
            get
            {
                if (fNonUIResolvers == null)
                    fNonUIResolvers = new TableResolverCollection();
                return fNonUIResolvers;
            }
        }

        #endregion INonUIResolvers 成员

        #region ISupportRecordLog 成员

        public void SetRecordDataPicker(string tableName, IRecordDataPicker picker)
        {
            if (fRecordLogs == null)
                fRecordLogs = new Dictionary<string, Tuple<IRecordDataPicker, List<RecordLogData>>>();
            if (!fRecordLogs.ContainsKey(tableName))
                fRecordLogs.Add(tableName, Tuple.Create(picker, new List<RecordLogData>()));
        }

        public IEnumerable<RecordLogData> GetRecordDatas(string tableName)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);

            if (fRecordLogs == null)
                return null;
            Tuple<IRecordDataPicker, List<RecordLogData>> recordLogInfo;
            if (fRecordLogs.TryGetValue(tableName, out recordLogInfo))
                return recordLogInfo.Item2;

            return null;
        }

        #endregion ISupportRecordLog 成员

        public MarcoConfigItem FilterSql { get; set; }

        protected virtual IParamBuilder GetDataRight(IInputData input)
        {
            if (SupportData && DataRight != null)
            {
                ListDataRightEventArgs e = new ListDataRightEventArgs(Context,
                    BaseGlobalVariable.Current.UserInfo, MainResolver);
                return DataRight.GetListSql(e);
            }
            return null;
        }

        protected virtual void Prepare()
        {
            if (fRecordLogs == null || fRecordLogs.Count == 0)
                return;
            if (fRecordLogs.ContainsKey(MainResolver.TableName))
                MainResolver.UpdatedRow += MainResolver_UpdatedRow;
            foreach (var childResolver in ChildResolvers)
            {
                if (fRecordLogs.ContainsKey(childResolver.TableName))
                    childResolver.UpdatedRow += MainResolver_UpdatedRow;
            }
        }

        private void MainResolver_UpdatedRow(object sender, UpdatingEventArgs e)
        {
            TableResolver resolver = sender.Convert<TableResolver>();
            Tuple<IRecordDataPicker, List<RecordLogData>> recordLogInfo;
            if (fRecordLogs.TryGetValue(resolver.TableName, out recordLogInfo))
            {
                RecordLogUtil.LogRecord(resolver, recordLogInfo.Item1, e, recordLogInfo.Item2);
            }
        }

        protected virtual void ApplyDatas(Transaction transaction)
        {
        }

        protected virtual void OnApplyDatas(ApplyDataEventArgs e)
        {
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.ApplyDataEvent, this, e);
        }

        protected virtual void OnCommittingData(CommittingDataEventArgs e)
        {
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.CommittingDataEvent, this, e);
        }

        protected virtual void OnCommittedData(CommittedDataEventArgs e)
        {
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.CommittedDataEvent, this, e);
        }

        protected void InternalApplyDatas(Transaction transaction)
        {
            ApplyDatas(transaction);
            OnApplyDatas(new ApplyDataEventArgs(transaction));
        }

        protected virtual void CommitData()
        {
            if (fNonUIResolvers == null)
                UpdateUtil.UpdateTableResolvers(Context, InternalApplyDatas, MainResolver);
            else
                UpdateUtil.UpdateTableResolvers(Context, InternalApplyDatas, MainResolver,
                    fNonUIResolvers);
        }

        protected virtual void FillData(IInputData input)
        {
            ParamBuilderContainer container = new ParamBuilderContainer();
            container.Add(GetDataRight(input));
            if (FilterSql != null)
            {
                string sql = Expression.Execute(FilterSql, this);
                container.Add(sql);
            }
            if (container.IsEmpty)
                MainResolver.Select();
            else
                MainResolver.Select(container);
        }

        public void Commit(IInputData input)
        {
            OnCommittingData(new CommittingDataEventArgs(input));
            CommitData();
            OnCommittedData(new CommittedDataEventArgs(input));
        }
    }
}