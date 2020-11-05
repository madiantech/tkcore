using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperateRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-07-30",
        Description = "功能权限中的子功能操作配置的操作权限（依靠功能权限的实现）")]
    internal class SubFuncOperateRightConfig : IConfigCreator<IOperateRight>
    {
        #region IConfigCreator<IOperateRight> 成员

        public IOperateRight CreateObject(params object[] args)
        {
            return new SubFuncOperateRight(FunctionKey);
        }

        #endregion

        [SimpleAttribute]
        public string FunctionKey { get; private set; }
    }
}
