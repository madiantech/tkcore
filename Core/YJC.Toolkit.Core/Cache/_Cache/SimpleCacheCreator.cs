using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheCreator(Author = "YJC", CreateDate = "2019-09-30", Description = "创建简单的内存缓存")]
    public sealed class SimpleCacheCreator : ICacheCreator
    {
        private const string REG_NAME = "Simple";
        private int fCapacity;

        public SimpleCacheCreator()
        {
        }

        public SimpleCacheCreator(int capacity)
            : this()
        {
            Capacity = capacity;
        }

        #region ICacheCreator 成员

        public ICache CreateCache(string cacheName)
        {
            return new SimpleCache(fCapacity);
        }

        #endregion ICacheCreator 成员

        public int Capacity
        {
            get
            {
                return fCapacity;
            }
            set
            {
                TkDebug.AssertArgument(value >= 0, "value", string.Format(ObjectUtil.SysCulture,
                    "value的值必须是正数，现在值为{0}是负数", value), this);
                fCapacity = value;
            }
        }
    }
}