using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    public static class HttpContextHelper
    {
        private static IHttpContextAccessor fAccessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => fAccessor.HttpContext;

        internal static void Configure(IHttpContextAccessor accessor)
        {
            fAccessor = accessor;
        }
    }
}