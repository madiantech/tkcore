using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class WorkflowContent : ToolkitConfig, IDisposable
    {
        private static readonly QName ROOT = QName.Get("Content");

        private readonly ContentItemCollection fContentItems;
        private DataRow fMainRow;

        internal WorkflowContent()
        {
            fContentItems = new ContentItemCollection();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        [ObjectElement(LocalName = "Item", IsMultiple = true,
            ObjectType = typeof(ContentItem))]
        internal ContentItemCollection ContentItems
        {
            get
            {
                return fContentItems;
            }
        }

        public int Count
        {
            get
            {
                return fContentItems.Count;
            }
        }

        public DataRow MainRow
        {
            get
            {
                TkDebug.AssertNotNull(fMainRow,
                    "只有在调用FillMainData或者FillData后，该数据才有值", this);
                return fMainRow;
            }
        }

        public TableResolver MainTableResolver
        {
            get
            {
                return fContentItems.MainItem.Resolver;
            }
        }

        public IEnumerable<TableResolver> TableResolvers
        {
            get
            {
                yield return MainTableResolver;
                foreach (var item in fContentItems)
                    if (!item.IsMain)
                        yield return item.Resolver;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in fContentItems)
                    item.DisposeObject();
            }
        }

        private void AssertMainRow(DataRow row)
        {
            // DataRowCollection rows = MainTableResolver.HostTable.Rows;
            //TkDebug.Assert(rows.Count == 1, string.Format(ObjectUtil.SysCulture,
            //    "注册名为{0}的Resolver因为设置为Main，所以只能选取一条记录，现在是{1}条，条件不符",
            //    fContentItems.MainItem.RegName, rows.Count), this);

            // fMainRow = rows[0];
            fMainRow = row;
        }

        internal void AddContentItem(ContentItem contentItem)
        {
            fContentItems.Add(contentItem);
        }

        private void AddContentItem(bool isMain, string orderBy, string tableName,
            IConfigCreator<TableResolver> resolver, string filterSql, RecordItem recordItem)
        {
            ContentItem contentItem = fContentItems[tableName];
            if (contentItem == null)
            {
                contentItem = new ContentItem(isMain, orderBy, tableName, resolver,
                    filterSql, HistoryFillAction.None, null);
                AddContentItem(contentItem);
            }
            contentItem.AddRecordItem(recordItem);
        }

        private void UpdateStatus(StepConfig stepConfig, string prefix)
        {
            fMainRow[prefix + "WfStatus"] = stepConfig.DisplayName;
            fMainRow[prefix + "StepName"] = stepConfig.Name;
        }

        internal void SetMainRowStatus(StepConfig stepConfig, string prefix)
        {
            fMainRow.BeginEdit();
            try
            {
                UpdateStatus(stepConfig, prefix);
            }
            finally
            {
                fMainRow.EndEdit();
            }
        }

        internal void EndMainRowStatus(string prefix, FinishType finishType)
        {
            fMainRow.BeginEdit();
            try
            {
                fMainRow[prefix + "WfIsEnd"] = finishType;
                switch (finishType)
                {
                    case FinishType.ReturnBegin:
                        break;

                    case FinishType.Abort:
                        fMainRow[prefix + "WfStatus"] = "终止";
                        fMainRow[prefix + "StepName"] = "__abort";
                        break;

                    case FinishType.OverTryTimes:
                        fMainRow[prefix + "WfStatus"] = "重试错误终止";
                        break;

                    case FinishType.Error:
                        fMainRow[prefix + "WfStatus"] = "错误终止";
                        break;

                    default:
                        break;
                }
            }
            finally
            {
                fMainRow.EndEdit();
            }
        }

        internal void SetAllMainRow(StepConfig stepConfig, string prefix, object workflowId)
        {
            fMainRow.BeginEdit();
            try
            {
                fMainRow[prefix + "WfId"] = workflowId;
                fMainRow[prefix + "IsApplyWf"] = 1;
                fMainRow[prefix + "WfIsEnd"] = 0;
                UpdateStatus(stepConfig, prefix);
            }
            finally
            {
                fMainRow.EndEdit();
            }
        }

        public void AddContentItem(string orderBy, string tableName, IConfigCreator<TableResolver> resolver,
            string filterSql, string[] keys, string[] values)
        {
            AddContentItem(false, orderBy, tableName, resolver, filterSql, keys, values);
        }

        public void AddContentItem(bool isMain, string orderBy, string tableName,
            IConfigCreator<TableResolver> resolver, string filterSql, string[] keys, string[] values)
        {
            RecordItem recordItem = new RecordItem(keys, values);
            AddContentItem(isMain, orderBy, tableName, resolver, filterSql, recordItem);
        }

        public void AddContentItem(string orderBy, string tableName, IConfigCreator<TableResolver> resolver,
            string filterSql, string key, string value)
        {
            RecordItem recordItem = new RecordItem(key, value);
            AddContentItem(false, orderBy, tableName, resolver, filterSql, recordItem);
        }

        public void AddContentItem(bool isMain, string orderBy, string tableName,
            IConfigCreator<TableResolver> resolver, string filterSql, string key, string value)
        {
            RecordItem recordItem = new RecordItem(key, value);
            AddContentItem(isMain, orderBy, tableName, resolver, filterSql, recordItem);
        }

        private void FillMainData(IDbDataSource source, bool isHistory)
        {
            TkDebug.AssertArgumentNull(source, "source不能为空", null);

            TkDebug.AssertNotNull(fContentItems.MainItem, "不存在主表", this);

            DataRow row = fContentItems.MainItem.FillData(source, isHistory);
            AssertMainRow(row);
        }

        private void FillData(IDbDataSource source, bool isHistory)
        {
            foreach (ContentItem item in ContentItems)
            {
                DataRow row = item.FillData(source, isHistory);
                if (item.IsMain)
                {
                    AssertMainRow(row);
                }
            }
        }

        internal void SetMainRow(DataRow row)
        {
            AssertMainRow(row);
        }

        public void Fill(FillContentMode mode, IDbDataSource source, bool isHistory = false)
        {
            switch (mode)
            {
                case FillContentMode.All:
                    FillData(source, isHistory);
                    break;

                case FillContentMode.MainOnly:
                    FillMainData(source, isHistory);
                    break;

                case FillContentMode.None:
                    break;
            }
        }

        public void FillWithMainData(FillContentMode mode, IDbDataSource source)
        {
            switch (mode)
            {
                case FillContentMode.All:
                    FillData(source, false);
                    break;

                case FillContentMode.MainOnly:
                case FillContentMode.None:
                    FillMainData(source, false);
                    break;
            }
        }

        /// <summary>
        /// 根据参数找到对应的TableResolver，如果找不到，或者找到没有生成TableResolver，均返回null
        /// </summary>
        /// <param name="regName"></param>
        /// <returns></returns>
        public TableResolver GetTableResolver(string regName)
        {
            ContentItem item = ContentItems[regName];
            if (item != null)
                return item.GetTableResolver();
            else
                return null;
        }

        /// <summary>
        /// 修改RegName，建议先remove，再Add
        /// </summary>
        /// <param name="oldRegName"></param>
        /// <param name="newRegName"></param>
        public void AddHistoryInfo(string oldRegName, IConfigCreator<TableResolver> resolverCreator)
        {
            //TableResolver resolver = null;
            ContentItem item = ContentItems[oldRegName];
            //if (item != null)
            //{
            //    resolver = item.GetTableResolver();
            //}
            //TkDebug.AssertNotNull(resolver, "要修改的注册名为{0}的TableResolver不存在", this);
            if (item != null)
            {
                ContentItems.Remove(item);
                item.AddHistoryInfo(resolverCreator);
                ContentItems.Add(item);
            }
        }

        public string CreateXml()
        {
            return this.WriteXml(WorkflowConst.WriteSettings, ROOT);
        }

        public static WorkflowContent ReadXml(string xml)
        {
            WorkflowContent content = new WorkflowContent();
            content.ReadXml(xml, WorkflowConst.ReadSettings, ROOT);
            return content;
        }
    }
}