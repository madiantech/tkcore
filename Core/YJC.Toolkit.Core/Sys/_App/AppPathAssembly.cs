using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    public sealed class AppPathAssembly : IEnumerable<Assembly>
    {
        private readonly LinkedList<Assembly> fBinAssembly;
        private readonly HashSet<string> fLoadedAssembly;
        private readonly LinkedList<InitAssemblyData> fInits;

        public AppPathAssembly()
        {
            fBinAssembly = new LinkedList<Assembly>();
            fLoadedAssembly = new HashSet<string>();
            fInits = new LinkedList<InitAssemblyData>();

            Add(ToolkitConst.TOOLKIT_CORE_NAME, ToolkitConst.TOOLKIT_CORE_ASSEMBLY);
        }

        #region IEnumerable<Assembly> 成员

        public IEnumerator<Assembly> GetEnumerator()
        {
            return fBinAssembly.GetEnumerator();
        }

        #endregion IEnumerable<Assembly> 成员

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable 成员

        internal HashSet<string> LoadedAssembly
        {
            get
            {
                return fLoadedAssembly;
            }
        }

        public bool Constains(AssemblyName name)
        {
            TkDebug.AssertArgumentNull(name, "name", this);

            return fLoadedAssembly.Contains(name.FullName);
        }

        //internal void AddLoadedAssembly(Assembly assembly)
        //{
        //    fLoadedAssembly.Add(assembly.FullName);
        //}

        internal void Add(AssemblyName name, Assembly assembly)
        {
            TkDebug.AssertArgumentNull(assembly, "assembly", this);

            if (!fLoadedAssembly.Contains(name.FullName))
            {
                fLoadedAssembly.Add(name.FullName);
                fBinAssembly.AddLast(assembly);
                Attribute attribute = Attribute.GetCustomAttribute(assembly,
                    typeof(InitializationAttribute));
                if (attribute != null)
                {
                    InitAssemblyData assemblyData = new InitAssemblyData(
                        attribute.Convert<InitializationAttribute>());
                    fInits.AddLast(assemblyData);
                }
            }
        }

        internal static void AddPlugInFactory(PlugInFactoryManager manager, Assembly assembly)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(assembly,
                typeof(AssemblyPlugInFactoryAttribute));
            if (attrs.Length > 0)
            {
                foreach (AssemblyPlugInFactoryAttribute attribute in attrs)
                {
                    BasePlugInFactory factory = ObjectUtil.CreateObject(
                        attribute.PlugInFactoryType).Convert<BasePlugInFactory>();
                    TkTrace.LogInfo($"添加工厂：{factory.Description}");
                    manager.Add(factory);
                }
            }
        }

        internal void AddPlugInFactory(PlugInFactoryManager manager)
        {
            foreach (Assembly assembly in fBinAssembly)
            {
                if (assembly == ToolkitConst.TOOLKIT_CORE_ASSEMBLY)
                    continue;
                AddPlugInFactory(manager, assembly);
            }
        }

        public IEnumerable<IInitialization> CreateInitializations()
        {
            var result = from item in fInits
                         orderby item.Priorty
                         select item.Initialization;
            return result;
        }
    }
}