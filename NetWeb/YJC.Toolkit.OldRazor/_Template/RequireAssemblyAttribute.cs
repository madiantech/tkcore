using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class RequireAssemblyAttribute : Attribute
    {
        public RequireAssemblyAttribute(IEnumerable<string> assemblies)
        {
            Assemblies = new HashSet<string>();
            if (assemblies != null)
            {
                foreach (string ns in assemblies)
                    Assemblies.Add(ns);
            }
        }

        public RequireAssemblyAttribute(params string[] assemblies)
            : this((IEnumerable<string>)assemblies)
        {
        }

        public RequireAssemblyAttribute(string assembly)
            : this(EnumUtil.Convert(assembly))
        {
        }

        public ISet<string> Assemblies { get; private set; }
    }
}
