using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [ObjectOperateRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-11-13", Description = "默认列表对象操作符的操作权限")]
    class SimpleListObjectOperateRightConfig : IConfigCreator<IObjectOperateRight>
    {
        [SimpleAttribute(DefaultValue = UpdateKind.All)]
        public UpdateKind Operators { get; internal set; }

        #region IConfigCreator<IOperateRight> 成员

        public IObjectOperateRight CreateObject(params object[] args)
        {
            return new SimpleListObjectOperateRight(Operators);
        }

        #endregion
    }
}
