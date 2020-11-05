using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperatorConfig(Author = "YJC", CreateDate = "2014-12-14", NamespaceType = NamespaceType.Toolkit,
        Description = "删除树节点操作")]
    internal class DeleteTreeNodeOperatorConfig : BaseOperatorConfig
    {
        public DeleteTreeNodeOperatorConfig()
            : base("删除节点", "icon-remove")
        {
        }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText ConfirmData { get; private set; }

        public override OperatorConfig CreateObject(params object[] args)
        {
            string info = RightConst.DELETE + ",AjaxUrl";
            string confirmData;
            if (ConfirmData == null)
                confirmData = "确定删除吗？";
            else
                confirmData = ConfirmData.ToString();

            return new OperatorConfig(RightConst.DELETE, OperatorCaption, OperatorPosition.Row,
                info, confirmData, IconClass, null);
        }
    }
}
