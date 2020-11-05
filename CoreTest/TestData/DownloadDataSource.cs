using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;
using YJC.Toolkit.Web;

namespace TestData
{
    [Source(Author = "YJC", CreateDate = "2020/6/8 16:11:12",
        Description = "数据源")]
    [SourceOutputPageMaker]
    internal class DownloadDataSource : ISource
    {
        public DownloadDataSource()
        {
        }

        public OutputData DoAction(IInputData input)
        {
            Uri url = new Uri("http://localhost:5000/c/~source/C/AliyunOSSUrl?File=eyJCdWNrZXROYW1lIjoiZWFtLWNsb3VkLW1lZGlhIiwiT2JqZWN0TmFtZSI6IjYyMzYxZTVhLWMyZTctNGI4OS05NWEzLTg5OTc3Njg0YmFhNiJ9");
            var res = NetUtil.HttpGet(url);
            var data = NetUtil.GetResponseData(res);
            return OutputData.Create("OK");
        }
    }
}