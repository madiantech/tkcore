using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [OperateRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2017-06-16", Description = "将操作权限链接起来，取各个权限的交集")]
    internal class CreateWorkflowOperateRightConfig : IConfigCreator<IOperateRight>
    {
        #region IConfigCreator<IOperateRight> 成员

        public IOperateRight CreateObject(params object[] args)
        {
            return new CreateWorkflowOperateRight
            {
                WfPrefix = WfPrefix,
                ReadOnly = ReadOnly
            };
        }

        #endregion IConfigCreator<IOperateRight> 成员

        [SimpleAttribute]
        public string WfPrefix { get; set; }

        [SimpleAttribute]
        public bool ReadOnly { get; set; }
    }
}