using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;
using YJC.Toolkit.Data;
using YJC.Toolkit.Cache;
using System.Text;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ModuleCreator(RegName = "wfp", Author = "YJC", CreateDate = "2017-10-28",
       Description = "")]
    [AlwaysCache, InstancePlugIn]
    internal class WorkflowProcessModuleCreator : IModuleCreator
    {
        public static readonly IModuleCreator Instance = new WorkflowProcessModuleCreator();

        private WorkflowProcessModuleCreator()
        {
        }

        #region IModuleCreator 成员

        public IModule Create(string source1)
        {
            string source = WebGlobalVariable.Request.Query["Source"];
            byte[] data = Convert.FromBase64String(source);
            string s = Encoding.UTF8.GetString(data);

            var processXml = s.ReadJsonFromFactory<IConfigCreator<ProcessXml>>(ProcessXmlConfigFactory.REG_NAME);

            return new WorkflowProcessModule(processXml.CreateObject());
        }

        #endregion IModuleCreator 成员
    }
}