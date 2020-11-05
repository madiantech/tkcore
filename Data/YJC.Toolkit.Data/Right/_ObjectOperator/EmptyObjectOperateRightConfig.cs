using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [ObjectOperateRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-11-13", Description = "不产生对象操作权限，配置的操作符将全部起效")]
    internal class EmptyObjectOperateRightConfig : IConfigCreator<IObjectOperateRight>
    {
        #region IConfigCreator<IOperateRight> 成员

        public IObjectOperateRight CreateObject(params object[] args)
        {
            return null;
        }

        #endregion
    }
}
