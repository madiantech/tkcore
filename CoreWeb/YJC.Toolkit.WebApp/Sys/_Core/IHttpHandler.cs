using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Sys
{
    public interface IHttpHandler
    {
        Task ProcessRequest(HttpContext context, RequestDelegate next, PathStringParser parser);

        string GetTemplateUrl(IPageStyle style, IPageData pageData);
    }
}