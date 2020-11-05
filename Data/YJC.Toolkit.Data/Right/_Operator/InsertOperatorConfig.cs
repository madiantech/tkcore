using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperatorConfig(Author = "YJC", CreateDate = "2014-12-14", NamespaceType = NamespaceType.Toolkit,
        Description = "新建操作")]
    internal class InsertOperatorConfig : BaseOperatorConfig
    {
        public InsertOperatorConfig()
            : base("新建", "icon-plus")
        {
        }

        [SimpleAttribute]
        public bool IsDialog { get; private set; }

        public override OperatorConfig CreateObject(params object[] args)
        {
            string info = IsDialog ? RightConst.INSERT_DIALOG : RightConst.INSERT;

            return new OperatorConfig(RightConst.INSERT, OperatorCaption, OperatorPosition.Global,
                info, null, IconClass, null);
        }
    }
}
