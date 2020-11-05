﻿using System;
using System.Reflection;

namespace YJC.Toolkit.Razor
{
    public class DefaultAssemblyDirectoryFormatter : IAssemblyDirectoryFormatter
    {
        public string GetAssemblyDirectory(Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            return Uri.UnescapeDataString(uri.Path);
        }
    }
}