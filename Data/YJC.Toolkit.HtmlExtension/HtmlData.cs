using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.HtmlExtension
{
    internal class HtmlData : ICacheDependencyCreator, IDisposable
    {
        private readonly ICacheDependency fDependency;

        public HtmlData(string fileName, string cacheFileName,
            string virtualPath, HtmlOption option)
        {
            fDependency = new FileInfoDependency(fileName);

            //string cacheFileName = HtmlUtil.GetCacheFileName(fileName);
            if (File.Exists(cacheFileName))
            {
                Data = File.ReadAllText(cacheFileName, ToolkitConst.UTF8);
            }
            else
            {
                Data = HtmlUtil.ReadHtmlFile(fileName, virtualPath, option);
                FileUtil.VerifySaveFile(cacheFileName, Data, ToolkitConst.UTF8);
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            fDependency.DisposeObject();
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        #region ICacheDependencyCreator 成员

        public ICacheDependency CreateCacheDependency()
        {
            return fDependency;
        }

        #endregion ICacheDependencyCreator 成员

        [SimpleElement]
        public string Data { get; private set; }
    }
}