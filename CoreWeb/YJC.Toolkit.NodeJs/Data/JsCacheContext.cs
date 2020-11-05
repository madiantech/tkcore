using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal static class JsCacheContext
    {
        public static TkDbContext GetDbContext()
        {
            TkDbContext context = DbContextUtil.CreateDefault();
            return context;
        }
    }
}
