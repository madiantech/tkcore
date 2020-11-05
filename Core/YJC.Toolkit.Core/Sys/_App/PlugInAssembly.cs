using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public class PlugInAssembly : IEnumerable<(AssemblyName Name, string FileName)>
    {
        private readonly Dictionary<string, (AssemblyName Name, string FileName)> fPlugIns;

        public PlugInAssembly(BaseAppSetting appSetting, AssemblyManager manager)
        {
            TkDebug.AssertArgumentNull(appSetting, nameof(appSetting), null);
            TkDebug.AssertArgumentNull(manager, nameof(manager), null);

            fPlugIns = new Dictionary<string, (AssemblyName Name, string FileName)>();
            if (!Directory.Exists(appSetting.PlugInPath))
                return;

            IEnumerable<string> files = Directory.EnumerateFiles(appSetting.PlugInPath,
                "*.dll", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    if (file.EndsWith(".resources.dll", StringComparison.OrdinalIgnoreCase))
                        continue;
                    AssemblyName name = AssemblyName.GetAssemblyName(file);
                    if (!manager.Contains(name))
                        fPlugIns.Add(name.FullName, (name, file));
                }
                catch
                {
                }
            }
        }

        public IEnumerator<(AssemblyName Name, string FileName)> GetEnumerator()
        {
            return fPlugIns.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Assembly LoadAssembly(AssemblyManager manager, bool noContext,
            (AssemblyName Name, string FileName) item)
        {
            TkDebug.AssertArgumentNull(manager, nameof(manager), this);
            TkDebug.AssertArgumentNull(item, nameof(item), this);

            if (!manager.Contains(item.Name))
            {
                Assembly assembly = noContext ? manager.LoadAssembly(item.Name, item.FileName)
                    : manager.LoadAssembly(item.Name);
                return assembly;
            }
            return null;
        }

        public Assembly TryLoadAssembly(AssemblyManager manager, bool noContext, string assemblyName)
        {
            TkDebug.AssertArgumentNull(manager, nameof(manager), this);
            TkDebug.AssertArgumentNullOrEmpty(assemblyName, nameof(assemblyName), this);

            if (fPlugIns.TryGetValue(assemblyName, out var value))
                return LoadAssembly(manager, noContext, value);

            return null;
        }
    }
}