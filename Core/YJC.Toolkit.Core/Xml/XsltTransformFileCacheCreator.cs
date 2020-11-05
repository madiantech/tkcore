using System.Xml.Xsl;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Xml
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2013-10-09",
       Description = "对xslt文件进行编译的缓存对象创建器")]
    [InstancePlugIn, AlwaysCache]
    internal sealed class XsltTransformFileCacheCreator : BaseCacheItemCreator
    {
        internal static BaseCacheItemCreator Instance = new XsltTransformFileCacheCreator();

        private XsltTransformFileCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            TransformSetting setting = args[0] as TransformSetting;
            FilesXmlResolver resolver = new FilesXmlResolver();
            XsltSettings xsltSetting = setting.NeedEvidence ? XsltSettings.TrustedXslt
                : XsltSettings.Default;
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(key, xsltSetting, resolver);
            return new XsltTransformCacheData(transform, resolver.GetFileNames());
        }
    }
}