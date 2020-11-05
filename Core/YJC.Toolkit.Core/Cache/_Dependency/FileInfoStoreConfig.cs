using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyStoreConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2019-09-18",
        Description = "跟踪单个文件变化(监控文件的修改时间)的缓存依赖", Author = "YJC", RegName = "FileInfo")]
    internal class FileInfoStoreConfig : IConfigCreator<IDistributeCacheDependency>
    {
        [SimpleAttribute]
        public long LastWriteTime { get; set; }

        [SimpleAttribute]
        public string FileName { get; set; }

        public IDistributeCacheDependency CreateObject(params object[] args)
        {
            return new FileInfoDependency(this);
        }
    }
}