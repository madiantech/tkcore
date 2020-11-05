using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [OperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-09-22",
        Description = "根据表SYS_SUB_FUNC的数据并参考角色权限的操作符配置")]
    internal class SubFuncOperatorsConfig : BaseSubFuncOperatorsConfig
    {
        public override IOperateRight CreateObject(params object[] args)
        {
            return new SubFuncOperateRight(FunctionKey);
        }
    }
}
