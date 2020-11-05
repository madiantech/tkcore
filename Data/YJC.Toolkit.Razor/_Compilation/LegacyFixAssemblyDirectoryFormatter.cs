using System;
using System.Reflection;

namespace YJC.Toolkit.Razor
{
    public class LegacyFixAssemblyDirectoryFormatter : IAssemblyDirectoryFormatter
    {
        public string GetAssemblyDirectory(Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            UriBuilder uriBuilder = new UriBuilder(codeBase);
            string assemlbyDirectory = Uri.UnescapeDataString(uriBuilder.Path + uriBuilder.Fragment);
            return assemlbyDirectory;
        }
    }
}