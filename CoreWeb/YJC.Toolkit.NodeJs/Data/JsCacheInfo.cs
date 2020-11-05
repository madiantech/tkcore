using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class JsCacheInfo : IReadObjectCallBack
    {
        [SimpleAttribute]
        public int Id { get; set; }

        [SimpleAttribute]
        public string ModuleName { get; set; }

        [SimpleAttribute]
        public long Ticks { get; set; }

        [SimpleAttribute]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool DevMode { get; set; }

        [SimpleAttribute]
        [TkTypeConverter(typeof(EnumIntTypeConverter), UseObjectType = true)]
        public DeviceType Device { get; set; }

        [SimpleAttribute]
        public string Model { get; set; }

        [SimpleAttribute]
        public string Template { get; set; }

        [SimpleAttribute]
        public string CacheDepend { get; set; }

        [SimpleAttribute]
        public long Hash { get; set; }

        [SimpleAttribute]
        public int ModelVersion { get; set; }

        public List<FileCacheInfo> FileCaches { get; set; }

        public void OnReadObject()
        {
            if (!string.IsNullOrEmpty(CacheDepend))
            {
                FileCaches = new List<FileCacheInfo>();
                FileCaches.ReadJson(CacheDepend);
            }
        }

        public static JsCacheInfo ReadFromDataRow(DataRow row)
        {
            JsCacheInfo info = new JsCacheInfo();
            info.ReadFromDataRow(row);
            return info;
        }

        public void WriteToRow(DataRow row)
        {
            if (FileCaches != null)
            {
                FileCaches.Sort();
                CacheDepend = FileCaches.WriteJson();
            }
            this.AddToDataRow(row);
        }
    }
}