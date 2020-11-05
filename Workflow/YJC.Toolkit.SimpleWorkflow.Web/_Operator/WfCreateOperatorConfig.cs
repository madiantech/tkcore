using YJC.Toolkit.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [OperatorConfig(Author = "YJC", CreateDate = "2017-06-14", NamespaceType = NamespaceType.Toolkit,
        Description = "修改操作")]
    internal class WfCreateOperatorConfig : BaseOperatorConfig
    {
        public const string ID = "_Create_WF";

        public WfCreateOperatorConfig()
            : base("提交", "icon-upload-alt")
        {
        }

        public WfCreateOperatorConfig(string workflowName, string iconClass, MultiLanguageText caption)
            : this()
        {
            TkDebug.AssertArgumentNullOrEmpty(workflowName, "workflowName", null);

            WorkflowName = workflowName;
            IconClass = iconClass;
            Caption = caption;

            OnReadObject();
        }

        [SimpleAttribute]
        public string WorkflowName { get; private set; }

        public override OperatorConfig CreateObject(params object[] args)
        {
            string url = "~/c/~source/C/WfCreate?WFName=" + WorkflowName;
            var result = new OperatorConfig(ID, OperatorCaption, OperatorPosition.Row,
                null, null, IconClass, new MarcoConfigItem(false, true, url))
            {
                UseKey = true
            };
            return result;
        }
    }
}