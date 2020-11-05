using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public sealed class AppCacheKey
    {
        private AppCacheKey()
        {
        }

        public AppCacheKey(string tenantId, string appName)
        {
            TenantId = tenantId;
            AppName = appName;
        }

        [SimpleAttribute]
        public string TenantId { get; private set; }

        [SimpleAttribute]
        public string AppName { get; private set; }

        public string ToJson()
        {
            return this.WriteJson();
        }

        public static AppCacheKey ReadFromString(string json)
        {
            TkDebug.AssertArgumentNullOrEmpty(json, "json", null);

            AppCacheKey key = new AppCacheKey();
            key.ReadJson(json);
            return key;
        }

        public static string GetKey(string tenantId, string appName)
        {
            AppCacheKey key = new AppCacheKey(tenantId, appName);
            return key.ToJson();
        }
    }
}