using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperateRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-07-18", Description = "不产生操作权限，配置的操作符将全部起效")]
    internal class EmptyOperateRightConfig : IConfigCreator<IOperateRight>
    {
        #region IConfigCreator<IOperateRight> 成员

        public IOperateRight CreateObject(params object[] args)
        {
            return null;
        }

        #endregion
    }
}
