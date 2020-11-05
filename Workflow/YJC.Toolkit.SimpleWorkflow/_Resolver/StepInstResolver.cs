using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class StepInstResolver : TableResolver
    {
        public const string TABLE_NAME = "WF_STEP_INST";

        /// <summary>
        /// 建构函数，设置附着的Xml文件。
        /// </summary>
        /// <param name="hostDataSet">附着的DataSet</param>
        public StepInstResolver(IDbDataSource source)
            : this(source, TABLE_NAME)
        {
        }

        /// <summary>
        /// 建构函数，设置附着的Xml文件。
        /// </summary>
        /// <param name="hostDataSet">附着的DataSet</param>
        protected StepInstResolver(IDbDataSource source, string tableName)
            : base(MetaDataUtil.CreateTableScheme("StepInst.xml", tableName), source)
        {
        }
    }
}