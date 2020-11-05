using System;
using System.Collections.Generic;
using System.Linq;

namespace YJC.Toolkit.Sys
{
    public static class AppUtil
    {
        public static string ResolveUrl(string url)
        {
            TkDebug.ThrowIfNoGlobalVariable();

            return BaseGlobalVariable.Current.ResolveUrl(url);
        }
    }
}