using System;
using System.Net;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public static class IMUtil
    {
        private static readonly DateTime Start = new DateTime(1970, 1, 1) + TimeZoneInfo.Local.BaseUtcOffset;

        public const string NULL_KEY = "__NULL_TK_KEY";

        internal static T GetFromUri<T>(Uri url, string modelName, T obj, bool downloadFile) where T : BaseResult
        {
            T result;
            if (downloadFile)
            {
                var response = NetUtil.HttpGet(url);
                DownloadData download = obj as DownloadData;
                TkDebug.AssertNotNull(download,
                    "Download标记为true时，返回值只能是DownloadData类型", obj);
                if (response.ContentType == "text/plain")
                {
                    download = NetUtil.ReadObjectFromResponse(response, modelName, download);
                }
                else
                {
                    byte[] data = NetUtil.GetResponseData(response);
                    download.FileData = data;
                }

                result = download as T;
            }
            else
                result = NetUtil.HttpGetReadJson(url, modelName, obj);
            if (result.IsError)
                throw new IMResultException(result);
            return result;
        }

        internal static T PostDataToUri<T>(Uri url, string modelName, string postData, T obj)
        {
            WebResponse response = NetUtil.HttpPost(url, postData, ContentTypeConst.JSON);
            return NetUtil.ReadObjectFromResponse(response, modelName, obj);
        }

        public static T PostToUri<T>(Uri url, string modelName, string postData,
            T obj) where T : BaseResult
        {
            T result = PostDataToUri(url, modelName, postData, obj);
            if (result.IsError)
                throw new IMResultException(result);
            return result;
        }

        public static DateTime ToDateTime(int createTime)
        {
            long tick = createTime * 10000000L;
            return Start + new TimeSpan(tick);
        }

        public static int ToCreateTime(DateTime date)
        {
            return (int)(date - Start).TotalSeconds;
        }

        public static T GetService<T>(IIMPlatform platform) where T : class
        {
            TkDebug.AssertArgumentNull(platform, "platform", null);

            var result = DispatchProxy.Create<T, IMProxy<T>>();
            result.Convert<IMProxy<T>>().SetProperties(platform);
            return result;
        }

        internal static T UploadFile<T>(Uri url, string fileName, byte[] fileData, T obj)
        {
            WebResponse response = NetUtil.FormUploadFile(url, "media", fileName, fileData);
            return NetUtil.ReadObjectFromResponse(response, null, obj);
        }
    }
}