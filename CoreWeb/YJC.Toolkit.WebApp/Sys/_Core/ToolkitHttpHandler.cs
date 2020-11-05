using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;
using YJC.Toolkit.Web.Page;

namespace YJC.Toolkit.Sys
{
    [HttpHandler(RegName = REG_NAME, Author = "YJC", CreateDate = "2018-07-16",
        Description = "默认的Toolkit的HttpHandler")]
    [AlwaysCache, InstancePlugIn]
    internal class ToolkitHttpHandler : IHttpHandler
    {
        public static readonly IHttpHandler Instance = new ToolkitHttpHandler();
        public const string REG_NAME = "c";

        private ToolkitHttpHandler()
        {
        }

        public Task ProcessRequest(HttpContext context, RequestDelegate next, PathStringParser parser)
        {
            PageSourceInfo info = null;
            parser.Parse((input) =>
            {
                info = ParseUrl(input, info);
            });
            if (info == null)
                return null;

            context.Items[WebUtil.SOURCE_INFO] = info;

            WebDefaultXmlConfig defaultConfig = WebGlobalVariable.WebCurrent.WebDefaultValue;
            WebBasePage page = null;
            try
            {
                if (info.IsContent)
                    page = new WebModuleContentPage(context, next, info);
                else
                    page = new WebModuleRedirectPage(context, next, info);
                return page.LoadPage();
            }
            catch (RedirectException ex)
            {
                context.Response.Redirect(ex.Url.ToString(), false);
                return Task.FromResult(0);
            }
            catch (ReLogOnException ex)
            {
                return HandleException(defaultConfig.ReLogOnHandler,
                    GetWebHandler(page, context, info), page, ex);
            }
            catch (ErrorPageException ex)
            {
                return HandleException(defaultConfig.ErrorPageHandler,
                    GetWebHandler(page, context, info), page, ex);
            }
            catch (ErrorOperationException ex)
            {
                return HandleException(defaultConfig.ErrorOperationHandler,
                    GetWebHandler(page, context, info), page, ex);
            }
            catch (ToolkitException ex)
            {
                return HandleException(defaultConfig.ToolkitHandler,
                    GetWebHandler(page, context, info), page, ex);
            }
            catch (Exception ex)
            {
                Exception innerEx = ex.InnerException;
                if (innerEx != null)
                    return ProcessException(context, defaultConfig, page, info, innerEx);
                else
                    return ProcessException(context, defaultConfig, page, info, ex);
            }
        }

        public string GetTemplateUrl(IPageStyle style, IPageData pageData)
        {
            if (style == null || pageData == null)
                return string.Empty;

            var info = pageData.SourceInfo;
            string source = info.Source;
            PageStyle pageStyle = style.Style;
            string styleString = pageStyle.ToString().ToLower(ObjectUtil.SysCulture);
            switch (pageStyle)
            {
                case PageStyle.Custom:
                    return $"c/{info.ModuleCreator}/{style}/{source}";

                case PageStyle.Insert:
                case PageStyle.List:
                case PageStyle.Update:
                case PageStyle.Delete:
                case PageStyle.Detail:
                    return $"c/{info.ModuleCreator}/{styleString}/{source}";

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return null;
            }
        }

        private static Task ProcessException(HttpContext context, WebDefaultXmlConfig defaultConfig,
            WebBasePage page, PageSourceInfo info, Exception exception)
        {
            if (exception is RedirectException redirectEx)
            {
                context.Response.Redirect(redirectEx.Url.ToString(), false);
                return Task.FromResult(0);
            }
            else if (exception is ReLogOnException)
                return HandleException(defaultConfig.ReLogOnHandler,
                    GetWebHandler(page, context, info), page, exception);
            else if (exception is ErrorPageException)
                return HandleException(defaultConfig.ErrorPageHandler,
                    GetWebHandler(page, context, info), page, exception);
            else if (exception is ErrorOperationException)
                return HandleException(defaultConfig.ErrorOperationHandler,
                    GetWebHandler(page, context, info), page, exception);
            else if (exception is ToolkitException)
                return HandleException(defaultConfig.ToolkitHandler,
                    GetWebHandler(page, context, info), page, exception);
            else
                return HandleException(defaultConfig.ExceptionHandler,
                    GetWebHandler(page, context, info), page, exception);
        }

        private static PageSourceInfo ParseUrl(string[] input, PageSourceInfo info)
        {
            if (input.Length >= 3)
            {
                bool isContent = true;
                string moduleCreator = input[0];
                if (string.IsNullOrEmpty(moduleCreator))
                    moduleCreator = "xml";
                else if (moduleCreator.StartsWith('~'))
                {
                    isContent = false;
                    moduleCreator = moduleCreator.Substring(1);
                }
                IPageStyle style = input[1].Value<PageStyleClass>((PageStyleClass)PageStyle.List);
                string source = string.Join('/', GetSourceParts(input));
                info = new PageSourceInfo(REG_NAME, moduleCreator, style, source, isContent);
            }

            return info;
        }

        private static IWebHandler GetWebHandler(IWebHandler page, HttpContext context,
            PageSourceInfo info)
        {
            return page ?? new ContextWebHandler(context, info);
        }

        private static IEnumerable<string> GetSourceParts(string[] input)
        {
            for (int i = 2; i < input.Length; ++i)
                yield return input[i];
        }

        private static Task HandleException(IConfigCreator<IExceptionHandler> exceptionHandler,
         IWebHandler handler, WebBasePage page, Exception ex)
        {
            IPageData pageData = page ?? handler;
            IExceptionHandler exHandler = exceptionHandler.CreateObject(pageData);
            return exHandler.HandleException(handler, page, ex);
        }
    }
}