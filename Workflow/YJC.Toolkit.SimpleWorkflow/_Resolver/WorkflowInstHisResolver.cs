using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WorkflowInstHisResolver : TableResolver
    {
        /// <summary>
        /// 建构函数，设置附着的Xml文件。
        /// </summary>
        /// <param name="hostDataSet">附着的DataSet</param>
        public WorkflowInstHisResolver(IDbDataSource source)
            : base(MetaDataUtil.CreateTableScheme("WorkflowInstHis.xml"), source)
        {
        }
    }
}