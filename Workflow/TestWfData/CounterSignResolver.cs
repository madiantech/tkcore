using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.SimpleWorkflow;
using YJC.Toolkit.Sys;

namespace TestWfData
{
    /// <summary>
    ///  会签表(WF_COUNTER_SIGN)的数据访问层类
    /// </summary>
    [Resolver(Author = "Chen Jiangyong", CreateDate = "2018-04-29",
        Description = "会签表(WF_COUNTER_SIGN)的数据访问层类")]
    public class CounterSignResolver : Tk5TableResolver
    {
        internal const string DATAXML = "Workflow/CounterSign.xml";

        /// <summary>
        /// 建构函数，设置附着的Xml文件。
        /// </summary>
        /// <param name="context">数据库连接上下文</param>
        /// <param name="source">附着的数据源</param>
        public CounterSignResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
            AutoUpdateKey = true;
        }

        /// <summary>
        /// 在表发生新建、修改和删除的时候触动。注意，千万不要删除base.OnUpdatingRow(e);
        /// UpdatingRow事件附着在基类该函数中。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnUpdatingRow(UpdatingEventArgs e)
        {
            base.OnUpdatingRow(e);

            switch (e.Status)
            {
                case UpdateKind.Insert:
                    break;

                case UpdateKind.Update:
                    break;

                case UpdateKind.Delete:
                    break;
            }
        }

        public DataRow InsertRow(WorkflowContent content, DataRow workflowRow, string oper)
        {
            DataRow row = NewRow();
            row.BeginEdit();
            row["Id"] = CreateUniId();
            row["MainWfId"] = workflowRow["Id"];
            if (content.MainTableResolver.KeyCount == 1)
                row["MainTableId"] = content.MainRow[content.MainTableResolver.KeyField];
            row["Operator"] = oper;
            //row["Approve"] = 1;
            UpdateTrackField(UpdateKind.Insert, row);
            row.EndEdit();
            return row;
        }
    }
}