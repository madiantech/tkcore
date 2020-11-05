using System;
using System.Collections.Generic;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    public abstract class BaseDistributeCacheItemCreator<T> : BaseCacheItemCreator, ICacheDataConverter
    {
        protected BaseDistributeCacheItemCreator()
        {
        }

        protected BaseDistributeCacheItemCreator(int capacity) : base(capacity)
        {
        }

        protected BaseDistributeCacheItemCreator(ICacheCreator cacheCreator) : base(cacheCreator)
        {
        }

        public override bool SupportDistributed { get => true; }

        public byte[] Convert(object data)
        {
            TkDebug.AssertArgumentNull(data, nameof(data), this);

            string json = data.WriteJson();
            return Encoding.UTF8.GetBytes(json);
        }

        public virtual object CreateEmptyData()
        {
            return ObjectUtil.CreateObjectWithCtor(typeof(T));
        }

        public object ReadData(byte[] data)
        {
            var obj = CreateEmptyData();
            TkDebug.AssertNotNull(obj, $"{nameof(CreateEmptyData)}返回的对象为空", this);

            string json = Encoding.UTF8.GetString(data);
            obj.ReadJson(json);
            return obj;
        }
    }
}