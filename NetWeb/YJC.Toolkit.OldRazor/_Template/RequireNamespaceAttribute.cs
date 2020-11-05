using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    /// <summary>
    /// Allows base templates to define required namespaces that will be automatically be
    /// added to generated templates.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class RequireNamespaceAttribute : Attribute
    {
        public RequireNamespaceAttribute(IEnumerable<string> namespaces)
        {
            Namespaces = new HashSet<string>();
            if (namespaces != null)
            {
                foreach (string ns in namespaces)
                    Namespaces.Add(ns);
            }
        }

        public RequireNamespaceAttribute(params string[] namespaces)
            : this((IEnumerable<string>)namespaces)
        {
        }

        public RequireNamespaceAttribute(string @namespace)
            : this(EnumUtil.Convert(@namespace))
        {
        }

        /// <summary>
        /// Gets the set of namespaces.
        /// </summary>
        public ISet<string> Namespaces { get; private set; }
    }
}