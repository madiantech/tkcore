using System;
using System.IO;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.HtmlExtension
{
    public class HtmlFilePageMaker : IPageMaker
    {
        private readonly string fFileName;

        public HtmlFilePageMaker(string virtualPath, string fileName)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);
            //TkDebug.AssertArgumentNullOrEmpty(virtualPath, "virtualPath", null);
            string path = string.IsNullOrEmpty(virtualPath) ? "." : virtualPath;
            fFileName = Path.Combine(WebAppSetting.Current.SolutionPath, "wwwroot", path, fileName);
            fFileName = Path.GetFullPath(fFileName);
            TkDebug.Assert(File.Exists(fFileName), string.Format(ObjectUtil.SysCulture,
                "文件{0}不存在，请确认", fFileName), null);

            FileName = fileName;
            VirtualPath = virtualPath;
            UseCache = true;
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            string content;
            if (UseCache)
            {
                string cacheKey = HtmlUtil.GetCacheKey(fFileName, VirtualPath);
                var data = CacheManager.GetItem(HtmlUtil.CACHE_NAME, cacheKey,
                    Tuple.Create(fFileName, VirtualPath, Option)).Convert<HtmlData>();
                content = data.Data;
            }
            else
                content = HtmlUtil.ReadHtmlFile(fFileName, VirtualPath, Option);
            return new SimpleContent(ContentTypeConst.HTML, content);
        }

        #endregion IPageMaker 成员

        public bool UseCache { get; set; }

        public string FileName { get; private set; }

        public string VirtualPath { get; private set; }

        public HtmlOption Option { get; set; }
    }
}