using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class StepInstResolver : TableResolver
    {
        public const string TABLE_NAME = "WF_STEP_INST";

        /// <summary>
        /// �������������ø��ŵ�Xml�ļ���
        /// </summary>
        /// <param name="hostDataSet">���ŵ�DataSet</param>
        public StepInstResolver(IDbDataSource source)
            : this(source, TABLE_NAME)
        {
        }

        /// <summary>
        /// �������������ø��ŵ�Xml�ļ���
        /// </summary>
        /// <param name="hostDataSet">���ŵ�DataSet</param>
        protected StepInstResolver(IDbDataSource source, string tableName)
            : base(MetaDataUtil.CreateTableScheme("StepInst.xml", tableName), source)
        {
        }
    }
}