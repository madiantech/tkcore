using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class XmlCreator : Creator
    {
        private readonly CreatorConfigItem fCreatorConfigItem;

        public XmlCreator(CreatorConfigItem config)
        {
            fCreatorConfigItem = config;
            FillMode = config.FillMode;
        }

        public override void AddContent(IDbDataSource source, WorkflowContent content, IParameter parameter)
        {
            foreach (var creatorContent in fCreatorConfigItem.ContentList)
            {
                TableResolver resolver = creatorContent.Resolver.CreateObject(source);
                bool isMain = creatorContent.IsMain;
                ContentItem contentItem = new ContentItem(isMain, creatorContent.OrderBy, resolver.TableName,
                    creatorContent.Resolver, creatorContent.FilterSql, creatorContent.HistoryMethod, creatorContent.HistoryResolver);
                foreach (RecordConfigItem record in creatorContent.RecordList)
                {
                    RecordItem recordItem = new RecordItem();
                    foreach (KeyConfigItem keyItem in record.KeyList)
                        recordItem.AddFieldItem(keyItem.NickName, parameter[keyItem.ParamName]);
                    contentItem.AddRecordItem(recordItem);
                }
                content.AddContentItem(contentItem);
            }
        }

        public override void SetWorkflowName(DataRow mainRow, IDbDataSource source)
        {
            WorkflowName = EvaluatorUtil.Execute<string>(fCreatorConfigItem.WorkflowName,
                ("row", mainRow));
        }
    }
}