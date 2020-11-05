using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using YJC.Toolkit.Log;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class SingleDbDelInsSource : BaseSingleDbSource, ICommitEvent,
        INonUIResolvers, ISupportRecordLog
    {
        private TableResolverCollection fNonUIResolvers;
        private string fTableName;
        private IRecordDataPicker fDataPicker;
        private List<RecordLogData> fLogResults;

        protected SingleDbDelInsSource()
        {
        }

        internal SingleDbDelInsSource(IBaseDbConfig config)
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

        protected override void OnSetMainResolver(TableResolver resolver)
        {
            resolver.UpdateMode = UpdateMode.DelIns;
        }

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
        }

        public void Commit(IInputData input)
        {
            OnCommittingData(new CommittingDataEventArgs(input));
            CommitData();
            OnCommittedData(new CommittedDataEventArgs(input));
        }

        public override OutputData DoAction(IInputData input)
        {
            if (input.IsPost)
            {
                FillData(input);
                Prepare();
                DataSet postDataSet = input.PostObject.Convert<DataSet>();
                MainResolver.Update(postDataSet, input);
                Commit(input);

                return OutputData.CreateToolkitObject(new KeyData("Id", string.Empty));
            }
            else
                throw new NotSupportedException();
        }
    }
}