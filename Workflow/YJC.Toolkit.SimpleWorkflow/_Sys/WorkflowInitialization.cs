using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WorkflowInitialization : IInitialization
    {
        #region IInitialization 成员

        public void AppStarting(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
        }

        public void AppStarted(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
            string fileName = Path.Combine(appsetting.XmlPath, "workflow.xml");
            TkDebug.Assert(File.Exists(fileName), string.Format(ObjectUtil.SysCulture,
                "工作流启动需要读取配置文件，文件路径为{0}，当前没有找到该文件", fileName), this);

            WorkflowConfigXml xml = new WorkflowConfigXml();
            xml.ReadXmlFromFile(fileName);
            WorkflowSetting.CreateWorkflowSetting(xml);
        }

        public void AppEnd(object application)
        {
        }

        #endregion IInitialization 成员
    }
}