using System.Text;
using System.Web;
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

        private static void WriteContent(HttpResponse response, IContent content)
        {
            TkDebug.ThrowIfNoAppSetting();
            if (WebAppSetting.WebCurrent.EnableCrossDomain)
            {
                string originHeader = WebGlobalVariable.Request.Headers["Origin"];
                if (!string.IsNullOrEmpty(originHeader))
                    response.AddHeader("Access-Control-Allow-Origin", originHeader);
                response.AddHeader("Access-Control-Allow-Credentials", "true");
            }

            response.ContentType = content.ContentType;
            string resp = content.Content;

            if (string.IsNullOrEmpty(resp))
                content.WritePage(response);
            else
            {
                Encoding encoding = content.ContentEncoding;
                if (encoding != null)
                    response.ContentEncoding = encoding;
                response.Write(resp);
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

            handler.Response.Redirect(url, false);
        }

        public static void WritePage(IMetaData metaData, ISource source, IPageMaker pageMaker,
            IWebHandler handler, OutputData outputData)
        {
            MetaDataUtil.SetMetaData(pageMaker, handler.Style, metaData);
            IContent content = pageMaker.WritePage(source, handler, outputData);

            InternalWebUtil.WriteContent(handler.Response, content);
        }

        public static void WritePage(IMetaData metaData, ISource source, IPageMaker pageMaker,
            IWebHandler handler)
        {
            PrepareSource(source, handler);
            OutputData outputData = CreateOutputData(metaData, source, handler.Style, handler);
            WritePage(metaData, source, pageMaker, handler, outputData);
        }
    }
}