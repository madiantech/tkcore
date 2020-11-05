using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperatorConfig(Author = "YJC", CreateDate = "2014-12-14", NamespaceType = NamespaceType.Toolkit,
        Description = "修改树节点操作")]
    internal class UpdateTreeNodeOperatorConfig : BaseOperatorConfig
    {
        public UpdateTreeNodeOperatorConfig()
            : base("修改节点", "icon-edit")
        {
        }

        [SimpleAttribute(DefaultValue = true)]
        public bool IsDialog { get; private set; }

        public override OperatorConfig CreateObject(params object[] args)
        {
            string info = IsDialog ? RightConst.UPDATE_DIALOG : RightConst.UPDATE;

            return new OperatorConfig(info, OperatorCaption, OperatorPosition.Row,
                info, null, IconClass, null);
        }
    }
}
