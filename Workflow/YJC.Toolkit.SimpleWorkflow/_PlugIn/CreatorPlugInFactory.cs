using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [XmlPlugIn("Workflow", typeof(WfCreatorXml), SearchPattern = "*Creator.xml")]
    [XmlBaseClass(CreatorConfigItem.BASE_CLASS, typeof(XmlCreator))]
    public class CreatorPlugInFactory : BaseXmlPlugInFactory
    {
        public const string REG_NAME = "_tk_Workflow_Creator";
        private const string DESCRIPTION = "工作流创建者对象的工厂";

        public CreatorPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}