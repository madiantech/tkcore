using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    internal static class JsCacheUtil
    {
        private static EmptyDbDataSource CreateDbSource()
        {
            EmptyDbDataSource source = new EmptyDbDataSource
            {
                Context = JsCacheContext.GetDbContext()
            };
            return source;
        }

        public static string GetModuleName(PageSourceInfo info)
        {
            return $"{info.ModuleCreator}-{info.Source}";
        }

        public static JsCacheInfo TryGetCacheInfo(PageSourceInfo info, string modelName, string templateName)
        {
            using (var source = CreateDbSource())
            using (var cacheResolver = new JsCacheResolver(source))
            {
                var moduleName = GetModuleName(info);
                var row = cacheResolver.TryFindRow(moduleName, WebAppSetting.WebCurrent.IsDevelopment,
                    modelName, templateName);
                if (row != null)
                {
                    return JsCacheInfo.ReadFromDataRow(row);
                }

                return null;
            }
        }

        public static bool VerfiyJsCache(JsCacheInfo cacheInfo)
        {
            if (cacheInfo.FileCaches == null || cacheInfo.FileCaches.Count == 0)
                return true;

            List<FileCacheInfo> current = (from item in cacheInfo.FileCaches
                                           select new FileCacheInfo(item.FileName)).ToList();

            return cacheInfo.FileCaches.SequenceEqual(current);
        }

        public static JsCacheInfo Save(PageSourceInfo info, string content, IEsModel model,
            IEsTemplate template, int modVersion, List<FileCacheInfo> cacheList)
        {
            using (var source = CreateDbSource())
            using (var cacheResolver = new JsCacheResolver(source))
            using (var storeResolver = new JsStoreResolver(source))
            {
                var moduleName = GetModuleName(info);
                DataRow row = cacheResolver.TryFindRow(moduleName, WebAppSetting.WebCurrent.IsDevelopment,
                    model.Name, template.Name);
                JsCacheInfo cacheInfo;
                DataRow storeRow = null;
                if (row != null)
                {
                    cacheResolver.SetCommands(AdapterCommand.Update);
                    cacheInfo = JsCacheInfo.ReadFromDataRow(row);
                    storeRow = storeResolver.TrySelectRowWithKeys(cacheInfo.Id);
                }
                else
                {
                    cacheResolver.SetCommands(AdapterCommand.Insert);
                    row = cacheResolver.NewRow();
                    cacheInfo = new JsCacheInfo
                    {
                        Id = cacheResolver.CreateUniId().Value<int>(),
                        ModuleName = GetModuleName(info),
                        DevMode = true,
                        Model = model.Name,
                        Template = template.Name
                    };
                }
                if (storeRow == null)
                {
                    storeResolver.SetCommands(AdapterCommand.Insert);
                    storeRow = storeResolver.NewRow();
                    storeRow["Id"] = cacheInfo.Id;
                }
                else
                    storeResolver.SetCommands(AdapterCommand.Update);

                cacheInfo.Ticks = DateTime.Now.Ticks;
                cacheInfo.ModelVersion = modVersion;
                cacheInfo.Hash = content.GetHashCode() ^ content.Length;
                cacheInfo.FileCaches = cacheList;
                cacheInfo.WriteToRow(row);
                storeRow["Content"] = Encoding.UTF8.GetBytes(content);
                UpdateUtil.UpdateTableResolvers(source.Context, null, cacheResolver, storeResolver);

                return cacheInfo;
            }
        }

        public static bool CheckTick(string id, string ticks)
        {
            string sql = "SELECT JC_TICKS FROM SYS_JS_CACHE";
            using (var context = JsCacheContext.GetDbContext())
            {
                IParamBuilder builder = SqlParamBuilder.CreateEqualSql(context, "JC_ID", TkDataType.Int, id);
                var dbTicks = DbUtil.ExecuteScalar(sql, context, builder).ToString();
                return dbTicks == ticks;
            }
        }

        public static string GetJsContent(string id)
        {
            string sql = "SELECT JS_CONTENT FROM SYS_JS_STORE";
            using (var context = JsCacheContext.GetDbContext())
            {
                IParamBuilder builder = SqlParamBuilder.CreateEqualSql(context, "JS_ID", TkDataType.Int, id);
                var content = (byte[])DbUtil.ExecuteScalar(sql, context, builder);
                return Encoding.UTF8.GetString(content);
            }
        }
    }
}