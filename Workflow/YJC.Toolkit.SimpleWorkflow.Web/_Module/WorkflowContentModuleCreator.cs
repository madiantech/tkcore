using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;
using YJC.Toolkit.Data;
using System.Text;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ModuleCreator(RegName = "wfc", Author = "YJC", CreateDate = "2017-06-01",
        Description = "创建工作流内容的Module Xml")]
    internal class WorkflowContentModuleCreator : IModuleCreator
    {
        #region IModuleCreator 成员

        public IModule Create(string source1)
        {
            string source = WebGlobalVariable.Request.Query["Source"];
            byte[] data = Convert.FromBase64String(source);
            string s = Encoding.UTF8.GetString(data);

            var contentXml = s.ReadJsonFromFactory<IConfigCreator<ContentXml>>(
                ContentXmlConfigFactory.REG_NAME);

            return new WorkflowContentModule(contentXml.CreateObject(), source1 != "content");
        }

        #endregion IModuleCreator 成员
    }
}