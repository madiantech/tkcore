using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    internal class JsCacheResolver : TableResolver
    {
        private const string DATAXML = "JsCache.xml";

        private static readonly string[] QueryFields =
            new string[] { "ModuleName", "DevMode", "Model", "Template" };

        public JsCacheResolver(IDbDataSource source)
            : base(MetaDataUtil.CreateTableScheme("JsCache.xml"), source)
        {
        }

        public DataRow TryFindRow(string moduleName, bool isDevMode,
            string modelName, string templateName)
        {
            return TrySelectRowWithParams(QueryFields, moduleName, isDevMode.Value<int>(),
                modelName, templateName);
        }
    }
}