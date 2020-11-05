using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class BaseEsTemplate : IEsTemplate
    {
        private readonly List<string> fPageNames;
        private BasePlugInAttribute fAttribute;

        protected BaseEsTemplate(string templatePath, params string[] pageNames)
        {
            TkDebug.AssertArgumentNullOrEmpty(templatePath, nameof(templatePath), null);
            TkDebug.AssertEnumerableArgumentNullOrEmpty(pageNames, nameof(pageNames), null);

            TemplatePath = templatePath;
            Arguments = "build.js";
            BundleFileName = "dist/bundle.js";
            fPageNames = new List<string>(pageNames);
        }

        public string TemplatePath { get; }

        public string Arguments { get; set; }

        public string BundleFileName { get; set; }

        public virtual string Name { get => Attribute.GetRegName(GetType()); }

        public virtual BasePlugInAttribute Attribute
        {
            get
            {
                if (fAttribute == null)
                {
                    fAttribute = System.Attribute.GetCustomAttribute(GetType(),
                        typeof(EsTemplateAttribute)).Convert<EsTemplateAttribute>();
                }
                return fAttribute;
            }
        }

        public void ExecuteNode(string workDir)
        {
            EsModelUtil.Execute(workDir, Arguments);
        }

        public IEnumerable<string> Generate(IEsModel model, HttpContext context, PageSourceInfo sourceInfo, IModule module)
        {
            List<string> result = new List<string>();
            foreach (string name in fPageNames)
            {
                var generator = model.GetPageGenerator(name);
                if (generator == null)
                    continue;
                var dependFiles = generator.CreateFile(context, sourceInfo, module);
                result.AddRange(dependFiles);
                var dependFile = generator.DependFile;
                if (!string.IsNullOrEmpty(dependFile))
                    result.Add(dependFile);
            }
            return ImmutableList.ToImmutableList(result.Distinct());
        }
    }
}