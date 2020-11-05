using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperatorConfig(Author = "YJC", CreateDate = "2014-12-14", NamespaceType = NamespaceType.Toolkit,
        Description = "修改操作")]
    internal class UpdateOperatorConfig : BaseOperatorConfig
    {
        public UpdateOperatorConfig()
            : base("修改", "icon-edit")
        {
        }

        [SimpleAttribute]
        public bool IsDialog { get; private set; }

        public override OperatorConfig CreateObject(params object[] args)
        {
            string info = IsDialog ? RightConst.UPDATE_DIALOG : RightConst.UPDATE;

            return new OperatorConfig(RightConst.UPDATE, OperatorCaption, OperatorPosition.Row,
                info, null, IconClass, null);
        }
    }
}
