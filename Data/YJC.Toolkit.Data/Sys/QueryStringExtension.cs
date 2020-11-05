using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public static class QueryStringExtension
    {
        public static Dictionary<string, string> ToDictionary(this IQueryString queryString)
        {
            TkDebug.AssertArgumentNull(queryString, nameof(queryString), null);

            return queryString.ToDictionary(p => p.Key, p => p.Value);
        }
    }
}