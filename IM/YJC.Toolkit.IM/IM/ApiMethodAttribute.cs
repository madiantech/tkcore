using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ApiMethodAttribute : Attribute
    {
        public ApiMethodAttribute(string uriPath)
        {
            TkDebug.AssertArgumentNullOrEmpty(uriPath, "uriPath", null);

            UriPath = uriPath;
            Method = HttpMethod.Get;
            UseAccessToken = true;
            ResultType = ResultType.Auto;
        }

        public string UriPath { get; private set; }

        public HttpMethod Method { get; set; }

        public bool UseAccessToken { get; set; }

        public string ResultKey { get; set; }

        public bool IsMultiple { get; set; }

        public Type ObjectType { get; set; }

        public bool UseConstructor { get; set; }

        public Type CollectionType { get; set; }

        public ResultType ResultType { get; set; }

        public string ResultModelName { get; set; }

        public Type PostType { get; set; }

        public string PostModelName { get; set; }

        public bool DownloadFile { get; set; }
    }
}