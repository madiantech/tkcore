using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    class HtmlUploadSource : ISource
    {
        private readonly static Dictionary<string, HashSet<string>> EXT_TABLE = CreateExtTable();
        private const int MAX_SIZE = 1000000;

        internal class ErrorResult
        {
            public ErrorResult(string message)
            {
                Error = 1;
                Message = message;
            }

            [SimpleAttribute(NamingRule = NamingRule.Camel)]
            public int Error { get; private set; }

            [SimpleAttribute(NamingRule = NamingRule.Camel)]
            public string Message { get; private set; }
        }

        internal class SucessResult
        {
            public SucessResult(string url)
            {
                Error = 0;
                Url = url;
            }

            [SimpleAttribute(NamingRule = NamingRule.Camel)]
            public int Error { get; private set; }

            [SimpleAttribute(NamingRule = NamingRule.Camel)]
            public string Url { get; private set; }
        }

        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            try
            {
                TkDebug.Assert(input.IsPost, "调用错误，只能使用Post方式", this);

                HttpRequest request = WebGlobalVariable.Request;
                HttpPostedFile file = request.Files == null ? null : request.Files["imgFile"];
                if (file == null)
                    throw new ToolkitException("请选择文件。", this);

                String dirName = input.QueryString["dir"];
                if (String.IsNullOrEmpty(dirName))
                    dirName = "image";
                var fileExts = EXT_TABLE[dirName];
                if (fileExts == null)
                    throw new ToolkitException("目录名不正确。", this);

                // 检查大小
                int maxLen = input.QueryString["MaxSize"].Value<int>();
                maxLen = maxLen > 0 ? Math.Min(maxLen, MAX_SIZE) : MAX_SIZE;
                if (file.InputStream == null || file.InputStream.Length > maxLen)
                    throw new ToolkitException("上传文件大小超过限制。", this);

                // 检查扩展名
                string fileExt = Path.GetExtension(file.FileName).ToLower();
                if (string.IsNullOrEmpty(fileExt) || !fileExts.Contains(fileExt.Substring(1)))
                {
                    throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                        "上传文件扩展名是不允许的扩展名。\n只允许{0}格式。", string.Join(",", fileExts)), this);
                }


                // 生成路径
                DateTime now = DateTime.Now;
                string ymd = now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
                string filePath = input.QueryString["UploadPath"];
                if (string.IsNullOrEmpty(filePath))
                    filePath = WebAppSetting.WebCurrent.UploadPath;
                else if (!Path.IsPathRooted(filePath))
                    filePath = Path.Combine(WebAppSetting.WebCurrent.SolutionPath, filePath);
                filePath = Path.Combine(filePath, "kindeditor", ymd);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                string fileName = now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
                file.SaveAs(Path.Combine(filePath, fileName));

                string urlPath = input.QueryString["VirtualPath"];
                if (string.IsNullOrEmpty(urlPath))
                    urlPath = WebAppSetting.WebCurrent.UploadVirtualPath;
                bool useRelativePath = input.QueryString["RelativePath"].Value<bool>();
                if (useRelativePath)
                    urlPath = VirtualPathUtility.MakeRelative(request.Url.AbsolutePath, urlPath);
                else
                    urlPath = WebUtil.ResolveUrl(urlPath);
                string url = UriUtil.TextCombine(urlPath, string.Format(ObjectUtil.SysCulture,
                    "{0}/{1}/{2}", "kindeditor", ymd, fileName));
                return OutputData.CreateToolkitObject(new SucessResult(url));
            }
            catch (Exception ex)
            {
                return OutputData.CreateToolkitObject(new ErrorResult(ex.Message));
            }
        }

        #endregion

        private static HashSet<string> CreateHashSet(string ext)
        {
            string[] data = ext.Split(',');
            HashSet<string> result = new HashSet<string>();
            foreach (string item in data)
                result.Add(item);

            return result;
        }

        private static Dictionary<string, HashSet<string>> CreateExtTable()
        {
            Dictionary<string, HashSet<string>> extTable = new Dictionary<string, HashSet<string>>();
            extTable.Add("image", CreateHashSet("gif,jpg,jpeg,png,bmp"));
            extTable.Add("flash", CreateHashSet("swf,flv"));
            extTable.Add("media", CreateHashSet("swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb"));
            extTable.Add("file", CreateHashSet("doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2"));

            return extTable;
        }
    }
}
