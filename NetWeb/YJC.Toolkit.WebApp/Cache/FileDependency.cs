using System;
using System.Web.Caching;

namespace YJC.Toolkit.Cache
{
    public sealed class FileDependency : ICacheDependency, IDisposable
    {
        private readonly CacheDependency fDependency;
        /// <summary>
        /// Initializes a new instance of the FileDependency class.
        /// </summary>
        public FileDependency(string fileName)
        {
            fDependency = new CacheDependency(fileName);
        }

        /// <summary>
        /// Initializes a new instance of the FileDependency class.
        /// </summary>
        public FileDependency(string[] fileNames)
        {
            fDependency = new CacheDependency(fileNames);
        }

        #region IDisposable 成员

        public void Dispose()
        {
            fDependency.Dispose();
        }

        #endregion

        #region ICacheDependency 成员

        bool ICacheDependency.HasChanged
        {
            get
            {
                return fDependency.HasChanged;
            }
        }

        #endregion

        public override string ToString()
        {
            return "监视文件内容是否改变的缓存依赖";
        }
    }
}
