using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class StepInstHisResolver : StepInstResolver
    {
        /// <summary>
        /// 建构函数，设置附着的Xml文件。
        /// </summary>
        /// <param name="hostDataSet">附着的DataSet</param>
        public StepInstHisResolver(IDbDataSource source)
            : base(source, "WF_STEP_INST_HIS")
        {
        }
    }
}