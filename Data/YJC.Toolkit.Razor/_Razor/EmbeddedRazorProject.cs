using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class EmbeddedRazorProject : TkRazorProject
    {
        public EmbeddedRazorProject(Type rootType)
        {
            TkDebug.AssertArgumentNull(rootType, nameof(rootType), null);

            Assembly = rootType.Assembly;
            RootNamespace = rootType.Namespace;
        }

        public EmbeddedRazorProject(Assembly assembly, string rootNamespace = "")
        {
            TkDebug.AssertArgumentNull(assembly, nameof(assembly), null);

            Assembly = assembly;
            RootNamespace = rootNamespace;
        }

        public Assembly Assembly { get; set; }

        public string RootNamespace { get; set; }

        public virtual string Extension { get; set; } = ".cshtml";

        public override Task<TkRazorProjectItem> GetItemAsync(string templateKey)
        {
            TkDebug.AssertArgumentNullOrEmpty(templateKey, nameof(templateKey), this);

            if (!templateKey.EndsWith(Extension))
            {
                templateKey = templateKey + Extension;
            }

            var item = new EmbeddedRazorProjectItem(Assembly, RootNamespace, templateKey);

            return Task.FromResult((TkRazorProjectItem)item);
        }

        public override Task<IEnumerable<TkRazorProjectItem>> GetImportsAsync(string templateKey)
        {
            return Task.FromResult(Enumerable.Empty<TkRazorProjectItem>());
        }
    }
}