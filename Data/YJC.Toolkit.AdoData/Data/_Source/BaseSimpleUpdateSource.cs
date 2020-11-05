using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Data.Constraint;
using YJC.Toolkit.Log;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseSimpleUpdateSource : BaseDbSource, INonUIResolvers,
        ICommitEvent, IEditEvent, ISupportMetaData, ISupportRecordLog
    {
        private TableResolverCollection fNonUIResolvers;
        private List<ResolverConfig> fUpdateResolvers;
        private IMetaData fMetaData;
        private Dictionary<string, (IRecordDataPicker Picker, List<RecordLogData> Data)> fRecordLogs;
        private readonly List<(string Master, string Detail, TableRelation Relation)> fRelations;

        protected BaseSimpleUpdateSource()
            : base()
        {
            fRelations = new List<(string Master, string Detail, TableRelation relation)>();
        }

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

        #region IEditEvent 成员

        public event EventHandler<FilledInsertEventArgs> FilledInsertTables
        {
            add
            {
                EventHandlers.AddHandler(EventConst.FilledInsertEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.FilledInsertEvent, value);
            }
        }

        public event EventHandler<PreparePostObjectEventArgs> PreparedPostObject
        {
            add
            {
                EventHandlers.AddHandler(EventConst.PreparedPostEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.PreparedPostEvent, value);
            }
        }

        #endregion IEditEvent 成员

        #region IDetailEvent 成员

        public event EventHandler<FillingUpdateEventArgs> FillingUpdateTables
        {
            add
            {
                EventHandlers.AddHandler(EventConst.FillingUpdateEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.FillingUpdateEvent, value);
            }
        }

        public event EventHandler<FilledUpdateEventArgs> FilledUpdateTables
        {
            add
            {
                EventHandlers.AddHandler(EventConst.FilledUpdateEvent, value);
            }
            remove
            {
                EventHandlers.RemoveHandler(EventConst.FilledUpdateEvent, value);
            }
        }

        #endregion IDetailEvent 成员

        #region ISource 成员

        public override OutputData DoAction(IInputData input)
        {
            try
            {
                PreparePostObject(input);

                FillCustomTables(input);
                OnFilledUpdateTables(new FilledUpdateEventArgs(input));

                PostData(input);
                Commit(input);

                return ReturnSucceedResult();
            }
            catch (WebPostException ex)
            {
                return OutputData.CreateToolkitObject(ex.CreateErrorResult());
            }
        }

        #endregion ISource 成员

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            if (TestPageStyleForMetaData(style))
                return UseMetaData;
            return false;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            if (TestPageStyleForMetaData(style))
            {
                fMetaData = metaData;

                //ITableSchemeEx scheme = metaData.GetTableScheme(MainResolver.TableName);
                //if (scheme != null)
                //{
                //    MainResolver.ReadMetaData(scheme);
                //    OnReadMetaData(MainResolver, style, scheme);
                //}
            }
        }

        #endregion ISupportMetaData 成员

        #region ISupportRecordLog 成员

        public void SetRecordDataPicker(string tableName, IRecordDataPicker picker)
        {
            if (fRecordLogs == null)
                fRecordLogs = new Dictionary<string, (IRecordDataPicker Picker, List<RecordLogData> Data)>();
            if (!fRecordLogs.ContainsKey(tableName))
                fRecordLogs.Add(tableName, (picker, new List<RecordLogData>()));
        }

        public IEnumerable<RecordLogData> GetRecordDatas(string tableName)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);

            if (fRecordLogs == null)
                return null;
            if (fRecordLogs.TryGetValue(tableName, out var recordLogInfo))
                return recordLogInfo.Data;

            return null;
        }

        #endregion ISupportRecordLog 成员

        public bool UseMetaData { get; set; }

        private static IEnumerable<TableResolver> GetUpdateResolvers(IEnumerable<ResolverConfig> configs)
        {
            foreach (ResolverConfig config in configs)
                yield return config.Resolver;
        }

        private IEnumerable<TableResolver> GetUpdateResolvers()
        {
            if (fUpdateResolvers == null)
                return Enumerable.Empty<TableResolver>();
            else
                return GetUpdateResolvers(fUpdateResolvers);
        }

        protected void InternalApplyDatas(Transaction transaction)
        {
            ApplyDatas(transaction);
            OnApplyDatas(new ApplyDataEventArgs(transaction));
        }

        private void PostData(IInputData input)
        {
            DataSet postDataSet = input.PostObject.Convert<DataSet>();
            FieldErrorInfoCollection errors = new FieldErrorInfoCollection();
            IEnumerable<ResolverConfig> configs = GetSafeUpdateResolvers(input);

            Prepare(configs);
            foreach (var config in configs)
            {
                MetaDataTableResolver metaResolver = config.Resolver as MetaDataTableResolver;
                if (metaResolver != null)
                    metaResolver.CheckFirstConstraints(input, errors);
            }
            foreach (var config in configs)
            {
                switch (config.Kind)
                {
                    case UpdateKind.Insert:
                        config.Resolver.Insert(postDataSet, input);
                        break;

                    case UpdateKind.Update:
                        config.Resolver.UpdateMode = config.Mode;
                        config.Resolver.Update(postDataSet, input);
                        break;

                    default:
                        break;
                }
            }
            foreach (var config in configs)
            {
                MetaDataTableResolver metaResolver = config.Resolver as MetaDataTableResolver;
                if (metaResolver != null)
                    metaResolver.CheckLaterConstraints(input, errors);
            }

            errors.CheckError();
        }

        protected virtual OutputData ReturnSucceedResult()
        {
            return OutputData.CreateToolkitObject(KeyData.Empty);
        }

        protected virtual void PreparePostObject(IInputData input)
        {
            DataSet postDataSet = input.PostObject.Convert<DataSet>();

            IEnumerable<ResolverConfig> configs = GetSafeUpdateResolvers(input);
            foreach (var item in configs)
            {
                if (item.Kind == UpdateKind.Update && item.Mode != UpdateMode.OneRow)
                    item.Resolver.PrepareDataSet(postDataSet);
            }
            foreach (var item in fRelations)
            {
                TableResolver master = GetResolver(configs, item.Master);
                TableResolver detail = GetResolver(configs, item.Detail);
                if (master == null || detail == null)
                    continue;

                item.Relation.SetSimpleFieldValue(master, detail);
            }
            OnPreparedPostObject(new PreparePostObjectEventArgs(input));
        }

        private TableResolver GetResolver(IEnumerable<ResolverConfig> configs, string tableName)
        {
            var result = (from item in configs
                          where item.Resolver.TableName == tableName
                          select item.Resolver).FirstOrDefault();
            return result;
        }

        protected virtual void OnPreparedPostObject(PreparePostObjectEventArgs e)
        {
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.PreparedPostEvent, this, e);
        }

        protected abstract bool TestPageStyleForMetaData(IPageStyle style);

        protected virtual void Prepare(IEnumerable<ResolverConfig> configs)
        {
            if (fRecordLogs == null || fRecordLogs.Count == 0)
                return;
            foreach (var config in configs)
            {
                if (fRecordLogs.ContainsKey(config.Resolver.TableName))
                    config.Resolver.UpdatedRow += Resolver_UpdatedRow;
            }
        }

        private void Resolver_UpdatedRow(object sender, UpdatingEventArgs e)
        {
            TableResolver resolver = sender.Convert<TableResolver>();
            if (fRecordLogs.TryGetValue(resolver.TableName, out var recordLogInfo))
            {
                RecordLogUtil.LogRecord(resolver, recordLogInfo.Picker, e, recordLogInfo.Data);
            }
        }

        protected virtual void OnReadMetaData(TableResolver resolver, IPageStyle style, ITableSchemeEx scheme)
        {
        }

        protected virtual void ApplyDatas(Transaction transaction)
        {
        }

        protected virtual void OnFilledUpdateTables(FilledUpdateEventArgs e)
        {
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.FilledUpdateEvent, this, e);
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
            //if (fUpdateActions != null)
            //{
            //    foreach (BaseUpdatedAction action in fUpdateActions)
            //        action.DoAction(DataSet, MainResolver);
            //}
            EventUtil.ExecuteEventHandler(EventHandlers, EventConst.CommittedDataEvent, this, e);
        }

        protected virtual void CommitData()
        {
            IEnumerable<TableResolver> updateResolvers = GetUpdateResolvers();

            if (fNonUIResolvers == null)
                UpdateUtil.UpdateTableResolvers(Context, InternalApplyDatas, updateResolvers);
            else
                UpdateUtil.UpdateTableResolvers(Context, InternalApplyDatas,
                    EnumUtil.Convert(updateResolvers, fNonUIResolvers));
        }

        protected abstract IEnumerable<ResolverConfig> CreateUpdateTableResolvers(IInputData input);

        protected abstract void FillCustomTables(IInputData input);

        protected IEnumerable<ResolverConfig> GetSafeUpdateResolvers(IInputData input)
        {
            if (fUpdateResolvers == null)
            {
                IEnumerable<ResolverConfig> configs = CreateUpdateTableResolvers(input);
                TkDebug.AssertNotNull(configs,
                    "CreateUpdateTableResolvers返回的TableResolvers不能为空", this);
                fUpdateResolvers = configs.ToList();
                int length = 0;
                foreach (ResolverConfig resolver in fUpdateResolvers)
                {
                    TkDebug.AssertNotNull(resolver, string.Format(ObjectUtil.SysCulture,
                        "CreateUpdateTableResolvers返回的第{0}个TableResolver为空", length), this);
                    ++length;
                    if (fMetaData != null && resolver.UseMeta)
                    {
                        MetaDataTableResolver metaResolver = resolver.Resolver as MetaDataTableResolver;
                        if (metaResolver != null)
                        {
                            InputDataProxy proxy = new InputDataProxy(input, (PageStyleClass)resolver.Style);
                            ITableSchemeEx scheme = fMetaData.GetTableScheme(metaResolver.TableName);
                            if (scheme != null)
                            {
                                metaResolver.ReadMetaData(scheme);
                                OnReadMetaData(metaResolver, proxy.Style, scheme);
                            }
                        }
                    }
                }
            }
            return fUpdateResolvers;
        }

        public void Commit(IInputData input)
        {
            OnCommittingData(new CommittingDataEventArgs(input));
            CommitData();
            OnCommittedData(new CommittedDataEventArgs(input));
        }

        public void AddRelation(string masterTableName, string detailTableName, TableRelation relation)
        {
            TkDebug.AssertArgumentNullOrEmpty(masterTableName, nameof(masterTableName), this);
            TkDebug.AssertArgumentNullOrEmpty(detailTableName, nameof(detailTableName), this);
            TkDebug.AssertArgumentNull(relation, nameof(relation), this);

            fRelations.Add((masterTableName, detailTableName, relation));
        }
    }
}