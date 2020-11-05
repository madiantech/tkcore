using System;
using System.Collections.Generic;
using System.Transactions;
using YJC.Toolkit.Log;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseSingleDbEditSource : BaseSingleDbDetailSource, ICommitEvent,
        INonUIResolvers, ISupportRecordLog
    {
        private TableResolverCollection fNonUIResolvers;
        private List<BaseUpdatedAction> fUpdateActions;
        private string fTableName;
        private IRecordDataPicker fDataPicker;
        private List<RecordLogData> fLogResults;

        protected BaseSingleDbEditSource()
        {
        }

        protected BaseSingleDbEditSource(IEditDbConfig config)
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
            fTableName = tableName;
            fDataPicker = picker;
        }

        public IEnumerable<RecordLogData> GetRecordDatas(string tableName)
        {
            return fLogResults;
        }

        #endregion ISupportRecordLog 成员

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

        protected virtual void Prepare()
        {
            if (fDataPicker == null)
                return;
            if (fTableName == null || fTableName == MainResolver.TableName)
            {
                fLogResults = new List<RecordLogData>();
                MainResolver.UpdatedRow += MainResolver_UpdatedRow;
            }
        }

        private void MainResolver_UpdatedRow(object sender, UpdatingEventArgs e)
        {
            if (fDataPicker == null)
                return;
            RecordLogUtil.LogRecord(MainResolver, fDataPicker, e, fLogResults);
            //var result = fDataPicker.PickData(e);
            //if (result != null)
            //{
            //    RecordLogData log = new RecordLogData(MainResolver.TableName,
            //        e.InvokeMethod, e.Status, result);
            //    fLogResults.Add(log);
            //}
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