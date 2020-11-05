using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal static class InternalWebUtil
    {
        public static bool GetReadOnly(PageStyle style)
        {
            switch (style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                    return false;

                case PageStyle.List:
                case PageStyle.Detail:
                    return true;

                default:
                    return true;
            }
        }

        private static void WriteJwt(HttpRequest request, HttpResponse response)
        {
            HttpContext context = HttpContextHelper.Current;
            var user = context.User;

            if (user is ToolkitClaimsPrincipal principal)
            {
                JWTUserInfo info = principal.UserInfo;
                if (info.UseRequestHeader)
                    response.Headers[info.HeaderName] = request.Headers[info.HeaderName];
                else
                    response.Headers[info.HeaderName] = info.Token;
            }
        }

        private static Task WriteContent(HttpRequest request, HttpResponse response, IContent content)
        {
            TkDebug.ThrowIfNoAppSetting();
            if (WebAppSetting.WebCurrent.EnableCrossDomain)
            {
                string originHeader = WebGlobalVariable.Request.Headers["Origin"];
                if (!string.IsNullOrEmpty(originHeader))
                    response.Headers["Access-Control-Allow-Origin"] = originHeader;
                response.Headers["Access-Control-Allow-Credentials"] = "true";
            }

            response.ContentType = content.ContentType;
            string resp = content.Content;

            WriteJwt(request, response);

            if (string.IsNullOrEmpty(resp))
                return content.WritePage(response);
            else
            {
                if (content.Headers != null)
                {
                    foreach (var item in content.Headers)
                    {
                        response.Headers[item.Key] = item.Value;
                    }
                }
                Encoding encoding = content.ContentEncoding;
                if (encoding != null)
                    return response.WriteAsync(content.Content, encoding);
                else
                    return response.WriteAsync(content.Content);
            }
            //response.End();
        }

        public static OutputData CreateOutputData(IMetaData metaData, ISource source,
            IPageStyle style, IPageData inputData)
        {
            MetaDataUtil.SetMetaData(source, style, metaData);
            OutputData outputData = source.DoAction(inputData);
            return outputData;
        }

        public static void PrepareSource(ISource source, IInputData input)
        {
            IPrepareSource prepare = source as IPrepareSource;
            if (prepare != null)
                prepare.Prepare(input);
        }

        public static void RedirectPage(IMetaData metaData, ISource source, IWebHandler handler,
            OutputData outputData, IRedirector redirector)
        {
            MetaDataUtil.SetMetaData(redirector, handler.Style, metaData);
            string url = redirector.Redirect(source, handler, outputData);
            TkDebug.AssertNotNullOrEmpty(url,
                "Redirector.Redirect函数返回的Url为空，不能重定向该地址", redirector);
            url = AppUtil.ResolveUrl(url);

            handler.Response.Redirect(url, false);
        }

        public static Task WritePage(IMetaData metaData, ISource source, IPageMaker pageMaker,
            IWebHandler handler, OutputData outputData)
        {
            MetaDataUtil.SetMetaData(pageMaker, handler.Style, metaData);
            IContent content = pageMaker.WritePage(source, handler, outputData);

            return WriteContent(handler.Request, handler.Response, content);
        }

        public static Task WritePage(IMetaData metaData, ISource source, IPageMaker pageMaker,
            IWebHandler handler)
        {
            PrepareSource(source, handler);
            OutputData outputData = CreateOutputData(metaData, source, handler.Style, handler);
            return WritePage(metaData, source, pageMaker, handler, outputData);
        }
    }
}