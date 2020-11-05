using System;
using System.Collections.Generic;
using System.Transactions;
using YJC.Toolkit.Log;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseMultipleDbEditSource : BaseMultipleDbDetailSource, ICommitEvent,
        INonUIResolvers, ISupportRecordLog
    {
        private TableResolverCollection fNonUIResolvers;
        private List<BaseUpdatedAction> fUpdateActions;
        private Dictionary<string, Tuple<IRecordDataPicker, List<RecordLogData>>> fRecordLogs;

        protected BaseMultipleDbEditSource()
        {
        }

        protected BaseMultipleDbEditSource(IEditDbConfig config)
            : base(config)
        {
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

        private void MainResolver_UpdatedRow(object sender, UpdatingEventArgs e)
        {
            TableResolver resolver = sender.Convert<TableResolver>();
            Tuple<IRecordDataPicker, List<RecordLogData>> recordLogInfo;
            if (fRecordLogs.TryGetValue(resolver.TableName, out recordLogInfo))
            {
                RecordLogUtil.LogRecord(resolver, recordLogInfo.Item1, e, recordLogInfo.Item2);
            }
        }

        protected void InternalApplyDatas(Transaction transaction)
        {
            ApplyDatas(transaction);
            OnApplyDatas(new ApplyDataEventArgs(transaction));
        }

        protected virtual void CommitData()
        {
            var updateResolvers = EnumUtil.Convert(MainResolver, ChildResolvers);
            if (fNonUIResolvers == null)
                UpdateUtil.UpdateTableResolvers(Context, InternalApplyDatas, updateResolvers);
            else
                UpdateUtil.UpdateTableResolvers(Context, InternalApplyDatas,
                    EnumUtil.Convert(updateResolvers, fNonUIResolvers));
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
            if (fUpdateActions != null)
            {
                foreach (BaseUpdatedAction action in fUpdateActions)
                    action.DoAction(DataSet, MainResolver);
            }
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.CommittedDataEvent, this, e);
        }

        public void Commit(IInputData input)
        {
            OnCommittingData(new CommittingDataEventArgs(input));
            CommitData();
            OnCommittedData(new CommittedDataEventArgs(input));
        }

        public void AddUpdatedAction(BaseUpdatedAction action)
        {
            if (action != null)
            {
                if (fUpdateActions == null)
                    fUpdateActions = new List<BaseUpdatedAction>();

                fUpdateActions.Add(action);
            }
        }
    }
}