using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WorkflowInstHisResolver : TableResolver
    {
        /// <summary>
        /// �������������ø��ŵ�Xml�ļ���
        /// </summary>
        /// <param name="hostDataSet">���ŵ�DataSet</param>
        public WorkflowInstHisResolver(IDbDataSource source)
            : base(MetaDataUtil.CreateTableScheme("WorkflowInstHis.xml"), source)
        {
        }
    }
}