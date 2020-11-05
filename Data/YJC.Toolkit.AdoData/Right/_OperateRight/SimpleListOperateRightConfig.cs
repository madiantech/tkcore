using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperateRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-11-25", Description = "默认列表操作符的操作权限")]
    class SimpleListOperateRightConfig : IConfigCreator<IOperateRight>
    {
        [SimpleAttribute(DefaultValue = UpdateKind.All)]
        public UpdateKind Operators { get; internal set; }

        #region IConfigCreator<IOperateRight> 成员

        public IOperateRight CreateObject(params object[] args)
        {
            return new SimpleListOperateRight(Operators);
        }

        #endregion
    }
}
