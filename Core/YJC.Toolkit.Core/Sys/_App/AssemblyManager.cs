using System;
using System.Collections.Generic;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    public class AssemblyManager
    {
        private readonly Dictionary<string, Assembly> fAssemblies;

        public AssemblyManager()
        {
            fAssemblies = new Dictionary<string, Assembly> {
                { ToolkitConst.TOOLKIT_CORE_ASSEMBLY.FullName, ToolkitConst.TOOLKIT_CORE_ASSEMBLY }
            };

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in loadedAssemblies)
            {
                string key = assembly.FullName;
                if (!fAssemblies.ContainsKey(key))
                    fAssemblies.Add(key, assembly);
            }
        }

        public bool Contains(string assemblyName)
        {
            TkDebug.AssertArgumentNullOrEmpty(assemblyName, nameof(assemblyName), this);

            return fAssemblies.ContainsKey(assemblyName);
        }

        public bool Contains(AssemblyName assemblyName)
        {
            TkDebug.AssertArgumentNull(assemblyName, nameof(assemblyName), this);

            return Contains(assemblyName.FullName);
        }

        public bool Contains(Assembly assembly)
        {
            TkDebug.AssertArgumentNull(assembly, nameof(assembly), this);

            return Contains(assembly.FullName);
        }

        public Assembly TryGetAssembly(string assemblyName)
        {
            TkDebug.AssertArgumentNullOrEmpty(assemblyName, nameof(assemblyName), this);

            if (fAssemblies.TryGetValue(assemblyName, out Assembly result))
                return result;

            return null;
        }

        public Assembly LoadAssembly(AssemblyName assemblyName)
        {
            TkDebug.AssertArgumentNull(assemblyName, nameof(assemblyName), this);

            if (fAssemblies.TryGetValue(assemblyName.FullName, out Assembly assembly))
                return assembly;
            assembly = Assembly.Load(assemblyName);
            fAssemblies.Add(assembly.FullName, assembly);
            return assembly;
        }

        public Assembly LoadAssembly(AssemblyName assemblyName, string fileName)
        {
            TkDebug.AssertArgumentNull(assemblyName, nameof(assemblyName), this);
            TkDebug.AssertArgumentNullOrEmpty(fileName, nameof(fileName), this);

            if (fAssemblies.TryGetValue(assemblyName.FullName, out Assembly assembly))
                return assembly;
            assembly = Assembly.LoadFrom(fileName);
            fAssemblies.Add(assembly.FullName, assembly);
            return assembly;
        }

        internal IEnumerable<Assembly> GetBinAssemblies(string appPath)
        {
            foreach (var assembly in fAssemblies.Values)
            {
                if (assembly == ToolkitConst.TOOLKIT_CORE_ASSEMBLY)
                    continue;
                string location = assembly.Location;
                if (!string.IsNullOrEmpty(location))
                {
                    if (location.StartsWith(appPath, StringComparison.CurrentCultureIgnoreCase))
                        yield return assembly;
                }
            }
        }
    }
}