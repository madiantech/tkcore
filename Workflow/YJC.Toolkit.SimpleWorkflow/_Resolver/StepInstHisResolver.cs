using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class StepInstHisResolver : StepInstResolver
    {
        /// <summary>
        /// �������������ø��ŵ�Xml�ļ���
        /// </summary>
        /// <param name="hostDataSet">���ŵ�DataSet</param>
        public StepInstHisResolver(IDbDataSource source)
            : base(source, "WF_STEP_INST_HIS")
        {
        }
    }
}