using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperatorConfig(Author = "YJC", CreateDate = "2014-12-14", NamespaceType = NamespaceType.Toolkit,
        Description = "新建树的子节点操作")]
    internal class InsertChildTreeNodeOperatorConfig : BaseOperatorConfig
    {
        private static readonly MarcoConfigItem fContent = new MarcoConfigItem(true, true,
            "~/CNewChild/~{CcSource}.c");
        public InsertChildTreeNodeOperatorConfig()
            : base("新建子节点", "icon-plus")
        {
        }

        [SimpleAttribute(DefaultValue = true)]
        public bool IsDialog { get; private set; }

        public override OperatorConfig CreateObject(params object[] args)
        {
            string info = IsDialog ? "Dialog" : string.Empty;

            return new OperatorConfig(RightConst.INSERT, OperatorCaption, OperatorPosition.Global,
                info, null, IconClass, fContent) { UseKey = true };
        }
    }
}
