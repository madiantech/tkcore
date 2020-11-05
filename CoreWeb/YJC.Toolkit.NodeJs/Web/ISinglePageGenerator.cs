using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public interface ISinglePageGenerator
    {
        string DependFile { get; }

        IEsModel Model { get; }

        string DestFile { get; }

        IEnumerable<string> CreateFile(HttpContext context, PageSourceInfo sourceInfo, IModule module);
    }
}