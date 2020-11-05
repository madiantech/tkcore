using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    [HttpHandler(RegName = REG_NAME, Author = "YJC", CreateDate = "2019-12-17",
        Description = "使用Node进行js打包的HttpHandler")]
    [AlwaysCache, InstancePlugIn]
    internal class JsHttpHandler : IHttpHandler
    {
        public static readonly IHttpHandler Instance = new JsHttpHandler();
        public const string REG_NAME = "js";

        private JsHttpHandler()
        {
        }

        public string GetTemplateUrl(IPageStyle style, IPageData pageData)
        {
            return string.Empty;
        }

        public Task ProcessRequest(HttpContext context, RequestDelegate next, PathStringParser parser)
        {
            //if (true)
            //{
            //    var resp2 = context.Response;
            //    resp2.StatusCode = 304;
            //    return Task.CompletedTask;
            //}
            //return WriteJsAsync(context, "alert('hello, world')");

            PageSourceInfo info = null;
            parser.Parse((input) =>
            {
                info = ParseUrl(input, info);
            });
            if (info == null)
                return WriteErrorJsAsync(context, $"{context.Request.Path.Value}不是规范的url");

            return GetJsScript(context, info);
        }

        private static Task GetJsScript(HttpContext context, PageSourceInfo info)
        {
            try
            {
                var request = context.Request;
                string id = request.Query["Id"];
                if (!string.IsNullOrEmpty(id))
                {
                    var tick = request.Query["_"];
                    if (JsCacheUtil.CheckTick(id, tick))
                    {
                        string content = JsCacheUtil.GetJsContent(id);
                        return WriteJsAsync(context, content);
                    }
                }
                IEsModel model = PlugInFactoryManager.CreateInstance<IEsModel>(
                    EsModelPlugInFactory.REG_NAME, request.Query["Model"]);
                IEsTemplate template = PlugInFactoryManager.CreateInstance<IEsTemplate>(
                    EsTemplatePlugInFactory.REG_NAME, request.Query["Template"]);
                string js = EsModelUtil.Execute(model, template, context, info);

                return WriteJsAsync(context, js);
            }
            catch (Exception ex)
            {
                return WriteErrorJsAsync(context, ex.Message, ex);
            }
        }

        private static Task WriteErrorJsAsync(HttpContext context, string errorMsg,
            Exception ex = null)
        {
            string js = $"alert('{errorMsg}')";
            return WriteJsAsync(context, js);
        }

        private static Task WriteJsAsync(HttpContext context, string js)
        {
            var resp = context.Response;
            resp.ContentType = ContentTypeConst.JAVA_SCRIPT;
            // Response.AddHeader('Last-Modified', entity.UpdatedAt.ToUniversalTime().ToString("R"));
            //resp.Headers["Last-Modified"] = DateTime.Now.ToUniversalTime().ToString("R");
            return resp.WriteAsync(js);
        }

        private void SetMetaData(Dictionary<PageStyle, IMetaData> dict, HttpContext context,
            PageSourceInfo info, IModule module, PageStyle style)
        {
            ContextWebHandler listHandler = new ContextWebHandler(context, info, (PageStyleClass)style);
            IMetaData metaData = module.CreateMetaData(listHandler);
            dict[style] = metaData;
        }

        private static PageSourceInfo ParseUrl(string[] input, PageSourceInfo info)
        {
            if (input.Length >= 3)
            {
                bool isContent = true;
                string moduleCreator = input[0];
                if (string.IsNullOrEmpty(moduleCreator))
                    moduleCreator = "xml";

                IPageStyle style = input[1].Value<PageStyleClass>((PageStyleClass)PageStyle.List);
                string source = string.Join('/', GetSourceParts(input));
                info = new PageSourceInfo(REG_NAME, moduleCreator, style, source, isContent);
            }

            return info;
        }

        private static IEnumerable<string> GetSourceParts(string[] input)
        {
            for (int i = 2; i < input.Length; ++i)
                yield return input[i];
        }
    }
}