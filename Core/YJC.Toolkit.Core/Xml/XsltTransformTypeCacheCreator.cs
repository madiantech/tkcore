using System;
using System.Xml.Xsl;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Xml
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2013-10-09",
       Description = "处理已经将xslt编译为的.net类型的缓存对象创建器")]
    [InstancePlugIn, AlwaysCache]
    internal class XsltTransformTypeCacheCreator : BaseCacheItemCreator
    {
        internal static BaseCacheItemCreator Instance = new XsltTransformTypeCacheCreator();

        private XsltTransformTypeCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            Type type = args[0] as Type;
            transform.Load(type);
            return new XsltTransformCacheData(transform, null);
        }
    }
}
