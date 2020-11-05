using System;
using System.IO;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class EmbeddedRazorProjectItem : TkRazorProjectItem
    {
        private readonly string fFullTemplateKey;

        public EmbeddedRazorProjectItem(Assembly assembly, string rootNamespace, string key)
        {
            TkDebug.AssertArgumentNull(assembly, nameof(assembly), null);
            TkDebug.AssertArgumentNullOrEmpty(key, nameof(key), null);

            Assembly = assembly;
            Key = key;

            RootNamespace = rootNamespace ?? string.Empty;
            fFullTemplateKey = assembly.GetName().Name + (!string.IsNullOrEmpty(RootNamespace) ? $".{RootNamespace}" : "") + $".{key}";
        }

        public EmbeddedRazorProjectItem(Type rootType, string key)
        {
            TkDebug.AssertArgumentNull(rootType, nameof(rootType), null);
            TkDebug.AssertArgumentNullOrEmpty(key, nameof(key), null);

            Key = key;
            Assembly = rootType.GetTypeInfo().Assembly;

            fFullTemplateKey = rootType.Namespace + "." + Key;
        }

        public Assembly Assembly { get; set; }

        public string RootNamespace { get; set; }

        public override string Key { get; }

        public override bool Exists
        {
            get
            {
                return Assembly.GetManifestResourceNames().Any(f => f == fFullTemplateKey);
            }
        }

        public override string BaseAssemblyPath => "Resource";

        public override Stream Read()
        {
            return Assembly.GetManifestResourceStream(fFullTemplateKey);
        }

        public override string CreateAssemblyName()
        {
            throw new NotImplementedException();
        }
    }
}