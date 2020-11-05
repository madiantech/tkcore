using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperateRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-07-30",
        Description = "根据Field的值以及功能权限中的子功能操作获得其交集的操作权限")]
    internal class SubFuncFieldOperateRightConfig : FieldOperateRightConfig
    {
        [SimpleAttribute]
        public string FunctionKey { get; private set; }

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
    }
}
