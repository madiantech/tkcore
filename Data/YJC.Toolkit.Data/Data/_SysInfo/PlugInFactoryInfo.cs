using System;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class PlugInFactoryInfo
    {
        internal PlugInFactoryInfo(BasePlugInFactory factory)
        {
            TkDebug.AssertArgumentNull(factory, "factory", null);

            Name = factory.Name;
            Description = factory.Description;
            Type type = factory.GetType();
            Count = factory.Count;
            TypeName = type.ToString();
            AssemblyName = GetFileName(type);
        }

        [SimpleAttribute]
        public string Name { get; private set; }

        [SimpleAttribute]
        public string Description { get; private set; }

        [SimpleAttribute]
        public string TypeName { get; private set; }

        [SimpleAttribute]
        public string AssemblyName { get; private set; }

        [SimpleAttribute]
        public int Count { get; private set; }

        internal static string GetFileName(Type type)
        {
            try
            {
                return Path.GetFileName(type.Assembly.CodeBase);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
