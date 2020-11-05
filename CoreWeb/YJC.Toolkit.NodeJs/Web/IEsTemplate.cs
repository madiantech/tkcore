using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public interface IEsTemplate
    {
        string Name { get; }

        string TemplatePath { get; }

        string BundleFileName { get; }

        IEnumerable<string> Generate(IEsModel model, HttpContext context, PageSourceInfo sourceInfo, IModule module);

        void ExecuteNode(string workDir);
    }
}