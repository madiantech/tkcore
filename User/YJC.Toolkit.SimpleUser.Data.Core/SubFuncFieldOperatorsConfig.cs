using System.Collections.Generic;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [OperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-09-22",
        Description = "根据表SYS_SUB_FUNC的数据并参考角色权限，再根据Field的值交叉获得的操作符配置")]
    internal class SubFuncFieldOperatorsConfig : BaseSubFuncOperatorsConfig
    {
        public override IOperateRight CreateObject(params object[] args)
        {
            SubFuncFieldOperateRight right = new SubFuncFieldOperateRight(FieldName, FunctionKey)
            {
                EmptyRowRights = Empty
            };
            if (Items != null)
                foreach (var item in Items)
                    right.AddItem(item);

            return right;
        }

        [SimpleAttribute]
        public string FieldName { get; protected set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string[] Empty { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item",
            UseConstructor = true)]
        public List<FieldOperateRightItem> Items { get; protected set; }
    }
}