using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    internal class DistributeData
    {
        private DistributeData()
        {
        }

        public DistributeData(ICacheDataConverter dataConverter, object cacheObject,
            IDistributeCacheDependency dependency)
        {
            CacheObject = dataConverter.Convert(cacheObject);
            Dependency = dependency;
            object storedObject = dependency.CreateStoredObject();
            DependencyObject = new DependencyStore { StoreObject = storedObject }.WriteJson();
        }

        [SimpleAttribute]
        public string DependencyObject { get; set; }

        [SimpleAttribute]
        public byte[] CacheObject { get; set; }

        public IDistributeCacheDependency Dependency { get; private set; }

        private void CreateDependency()
        {
            var config = DependencyObject.ReadJsonFromFactory<IConfigCreator<IDistributeCacheDependency>>(
                CacheDependencyStoreConfigFactory.REG_NAME);
            Dependency = config.CreateObject();
        }

        public object CreateCacheObject(ICacheDataConverter dataConverter)
        {
            return dataConverter.ReadData(CacheObject);
        }

        public byte[] ToDistributeData()
        {
            string json = this.WriteJson();
            return Encoding.UTF8.GetBytes(json);
        }

        public static DistributeData FromDistributeData(byte[] data)
        {
            string json = Encoding.UTF8.GetString(data);
            DistributeData result = new DistributeData();
            result.ReadJson(json);
            result.CreateDependency();
            return result;
        }
    }
}