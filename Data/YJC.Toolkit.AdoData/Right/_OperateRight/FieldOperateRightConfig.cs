using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperateRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-08-07", Description = "根据Field的值配置的操作权限")]
    class FieldOperateRightConfig : IConfigCreator<IOperateRight>
    {
        #region IConfigCreator<IOperateRight> 成员

        public virtual IOperateRight CreateObject(params object[] args)
        {
            FieldOperateRight right = new FieldOperateRight(FieldName)
            {
                EmptyRowRights = Empty
            };
            if (Items != null)
                foreach (var item in Items)
                    right.AddItem(item);

            return right;
        }

        #endregion

        [SimpleAttribute]
        public string FieldName { get; protected set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string[] Empty { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item",
            UseConstructor = true)]
        public List<FieldOperateRightItem> Items { get; protected set; }
    }
}
