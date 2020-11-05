using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal sealed class ContentItem : IRegName, IDisposable
    {
        private readonly List<RecordItem> fRecordItems;
        private TableResolver fTableResolver;

        public ContentItem()
        {
            fRecordItems = new List<RecordItem>();
        }

        internal ContentItem(bool isMain, string orderBy, string tableName,
            IConfigCreator<TableResolver> resolver, string filterSql,
            HistoryFillAction historyMethod, IConfigCreator<TableResolver> historyResolver)
            : this()
        {
            IsMain = isMain;
            RegName = tableName;
            ResolverCreator = resolver;
            OrderBy = orderBy;
            FilterSql = filterSql;
            HistoryMethod = historyMethod;
            HisResolverCreator = historyResolver;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            fTableResolver.DisposeObject();

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        #region IRegName 成员

        [SimpleAttribute(Required = true)]
        public string RegName { get; private set; }

        #endregion IRegName 成员

        [SimpleAttribute]
        public bool IsMain { get; private set; }

        [SimpleAttribute]
        public string OrderBy { get; private set; }

        [TagElement]
        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        public IConfigCreator<TableResolver> ResolverCreator { get; private set; }

        [SimpleAttribute(DefaultValue = HistoryFillAction.None)]
        public HistoryFillAction HistoryMethod { get; private set; }

        [TagElement]
        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        public IConfigCreator<TableResolver> HisResolverCreator { get; private set; }

        [ObjectElement(LocalName = "Record", IsMultiple = true,
            ObjectType = typeof(RecordItem))]
        internal IEnumerable<RecordItem> RecordItems
        {
            get
            {
                return fRecordItems;
            }
        }

        [SimpleElement(NamespaceType.Toolkit)]
        public string FilterSql { get; private set; }

        public TableResolver Resolver
        {
            get
            {
                TkDebug.AssertNotNull(fTableResolver, "要先调用FillData才有值", this);

                return fTableResolver;
            }
        }

        internal TableResolver GetTableResolver()
        {
            return fTableResolver;
        }

        internal void AddHistoryInfo(IConfigCreator<TableResolver> resolverCreator)
        {
            HisResolverCreator = resolverCreator;
            if (HisResolverCreator != null && HistoryMethod == HistoryFillAction.None)
                HistoryMethod = HistoryFillAction.Perhaps;
        }

        internal void AddRecordItem(RecordItem recordItem)
        {
            fRecordItems.Add(recordItem);
        }

        internal DataRow FillData(IDbDataSource source, bool isHistory = false)
        {
            TkDbContext context = source.Context;
            fTableResolver = ResolverCreator.CreateObject(source);
            TableResolver historyResolver = null;
            if (isHistory && HisResolverCreator != null && HistoryMethod != HistoryFillAction.None)
                historyResolver = HisResolverCreator.CreateObject(source);
            bool canHistory = isHistory && historyResolver != null;
            string filterSql = string.Empty;
            if (!string.IsNullOrEmpty(FilterSql))
                filterSql = EvaluatorUtil.Execute<string>(FilterSql, ("Source", source));
            foreach (RecordItem recordItem in RecordItems)
            {
                int length = recordItem.FieldItems.Count;
                string[] keys = new string[length];
                object[] values = new object[length];

                for (int i = 0; i < length; i++)
                {
                    keys[i] = recordItem.FieldItems[i].Key;
                    values[i] = recordItem.FieldItems[i].Value;
                }
                if (IsMain)
                {
                    if (canHistory)
                    {
                        DataRow row = null;
                        switch (HistoryMethod)
                        {
                            case HistoryFillAction.None:
                                row = fTableResolver.SelectRowWithParams(filterSql, OrderBy, keys, values);
                                break;

                            case HistoryFillAction.Perhaps:
                                row = historyResolver.TrySelectRowWithParams(filterSql, OrderBy, keys, values);
                                if (row == null)
                                    row = fTableResolver.SelectRowWithParams(filterSql, OrderBy, keys, values);
                                else
                                    historyResolver.HostTable.TableName = RegName;
                                break;

                            case HistoryFillAction.History:
                                row = historyResolver.SelectRowWithParams(filterSql, OrderBy, keys, values);
                                historyResolver.HostTable.TableName = RegName;
                                break;
                        }
                        historyResolver.DisposeObject();
                        return row;
                    }
                    else
                        return fTableResolver.SelectRowWithParams(filterSql, OrderBy, keys, values);
                }
                else
                {
                    if (canHistory && HistoryMethod == HistoryFillAction.History)
                    {
                        historyResolver.SelectWithParams(filterSql, OrderBy, keys, values);
                        if (historyResolver.HostTable != null)
                            historyResolver.HostTable.TableName = RegName;
                    }
                    else
                    {
                        fTableResolver.SelectWithParams(filterSql, OrderBy, keys, values);
                    }
                    historyResolver.DisposeObject();
                }
            }
            return null;
        }
    }
}