using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2013-11-02",
        Description = "Tk5 DataXml的缓存对象创建器")]
    internal class Tk5DataXmlCacheCreator : BaseDistributeCacheItemCreator<InternalTk5DataXml>
    {
        public override object Create(string key, params object[] args)
        {
            InternalTk5DataXml xml = new InternalTk5DataXml();
            xml.ReadXmlFromFile(key);

            return xml;
        }

        public override string TransformCacheKey(string key)
        {
            //return key.ToLower(ObjectUtil.SysCulture);
            return key;
        }
    }
}