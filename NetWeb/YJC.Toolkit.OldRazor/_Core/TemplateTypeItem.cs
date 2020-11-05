using System;
using System.Collections.Generic;

namespace YJC.Toolkit.Razor
{
    internal class TemplateTypeItem
    {
        private readonly HashSet<string> fNamespaces;
        private readonly HashSet<string> fAssemblies;

        public TemplateTypeItem(Type type)
        {
            fNamespaces = new HashSet<string>();
            fAssemblies = new HashSet<string>();

            Attribute[] attrs = Attribute.GetCustomAttributes(type);
            foreach (Attribute attr in attrs)
            {
                RequireAssemblyAttribute assemblyAttr = attr as RequireAssemblyAttribute;
                if (assemblyAttr != null)
                    foreach (string item in assemblyAttr.Assemblies)
                        fAssemblies.Add(item);

                RequireNamespaceAttribute nsAttr = attr as RequireNamespaceAttribute;
                if (nsAttr != null)
                    foreach (string item in nsAttr.Namespaces)
                        fNamespaces.Add(item);
            }
        }

        public IEnumerable<string> Namespaces
        {
            get
            {
                return fNamespaces;
            }
        }

        public IEnumerable<string> Assemblies
        {
            get
            {
                return fAssemblies;
            }
        }
    }
}
