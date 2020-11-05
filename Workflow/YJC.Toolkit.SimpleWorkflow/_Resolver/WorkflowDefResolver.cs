using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WorkflowDefResolver : TableResolver
    {
        /// <summary>
        /// 建构函数，设置附着的Xml文件。
        /// </summary>
        /// <param name="hostDataSet">附着的DataSet</param>
        public WorkflowDefResolver(IDbDataSource source)
            : base(MetaDataUtil.CreateTableScheme("WorkflowDef.xml"), source)
        {
        }

        public void Insert(WorkflowConfig config)
        {
            TkDebug.AssertArgumentNull(config, "config", this);

            SetCommands(AdapterCommand.Insert);
            DataRow row = NewRow();
            row.BeginEdit();
            try
            {
                row["Id"] = CreateUniId();
                row["ShortName"] = config.Name;
                row["Name"] = config.DisplayName;
                row["Description"] = config.Description;
                row["Content"] = config.CreateXml();
                row["Version"] = config.Version;
                UpdateTrackField(UpdateKind.Insert, row);
            }
            finally
            {
                row.EndEdit();
            }

            UpdateDatabase();
        }

        public void Update(WorkflowConfig config)
        {
            TkDebug.AssertArgumentNull(config, "config", this);

            SetCommands(AdapterCommand.Update);
            DataRow row = SelectRowWithKeys(config.Name);
            row.BeginEdit();
            try
            {
                row["Name"] = config.DisplayName;
                row["Description"] = config.Description;
                row["Content"] = config.CreateXml();
                row["Version"] = config.Version;
                UpdateTrackField(UpdateKind.Insert, row);
            }
            finally
            {
                row.EndEdit();
            }

            UpdateDatabase();
        }
    }
}