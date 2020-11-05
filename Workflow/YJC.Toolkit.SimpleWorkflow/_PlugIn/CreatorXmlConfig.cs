using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [CreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-04-29",
        Description = "通过配置文件创建Creator")]
    internal class CreatorXmlConfig : IConfigCreator<Creator>
    {
        #region IConfigCreator<Creator> 成员

        public Creator CreateObject(params object[] args)
        {
            TkDebug.ThrowIfNoAppSetting();

            string fileName = Path.Combine(BaseAppSetting.Current.XmlPath,
                "Workflow", FileName);
            TkDebug.Assert(File.Exists(fileName), string.Format(ObjectUtil.SysCulture,
                "文件{0}没有在Workflow对应的目录下找到，请确认", FileName), null);
            WfCreatorXml xml = new WfCreatorXml();
            xml.ReadXmlFromFile(fileName);
            return new XmlCreator(xml.Creator);
        }

        #endregion IConfigCreator<Creator> 成员

        [SimpleAttribute(Required = true)]
        public string FileName { get; private set; }
    }
}