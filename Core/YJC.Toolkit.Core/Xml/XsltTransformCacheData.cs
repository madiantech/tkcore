using System.Xml.Xsl;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Xml
{
    internal sealed class XsltTransformCacheData : ICacheDependencyCreator
    {
        private readonly string[] fFileNames;
        //private readonly CacheDependencyAttribute fAttribute;
        private readonly ICacheDependency fDependency;

        /// <summary>
        /// Initializes a new instance of the XsltTransformCacheData class.
        /// </summary>
        /// <param name="fileNames"></param>
        public XsltTransformCacheData(XslCompiledTransform transform, string[] fileNames)
        {
            fFileNames = fileNames;
            Transform = transform;
            //fAttribute = fileNames == null ? AlwaysCacheAttribute.Attribute :
            //    new FilesCacheAttribute(fFileNames, FilePathPosition.AbsolutePath);
            fDependency = AlwaysDependency.Dependency;
        }

        public XslCompiledTransform Transform { get; private set; }

        #region ICacheDependencyCreator 成员

        public ICacheDependency CreateCacheDependency()
        {
            return fDependency;
        }

        #endregion
    }
}
