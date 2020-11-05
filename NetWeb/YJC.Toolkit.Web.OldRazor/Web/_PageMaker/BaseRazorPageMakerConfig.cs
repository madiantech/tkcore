using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class BaseRazorPageMakerConfig
    {
        [SimpleAttribute]
        public string Template { get; protected set; }

        [SimpleAttribute]
        public string RazorFile { get; protected set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(RazorDataConfigFactory.REG_NAME)]
        public IConfigCreator<object> RazorData { get; set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Script")]
        public List<ScriptConfig> Scripts { get; protected set; }

        [SimpleElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Assembly")]
        public List<string> Assemblies { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public RetUrlConfig RetUrl { get; set; }

        public void Assign(BaseRazorPageMakerConfig config)
        {
            if (config == null)
                return;

            if (!string.IsNullOrEmpty(config.Template))
                Template = config.Template;
            if (!string.IsNullOrEmpty(config.RazorFile))
                RazorFile = config.RazorFile;
            if (config.RazorData != null)
                RazorData = config.RazorData;
            if (config.Scripts != null)
            {
                if (Scripts != null)
                    Scripts.AddRange(config.Scripts);
                else
                    Scripts = config.Scripts;
            }
            if (config.RetUrl != null)
                RetUrl = config.RetUrl;
            if (config.Assemblies != null)
                Assemblies = config.Assemblies;
        }
    }
}