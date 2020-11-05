using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PostPageMakerAttribute : BasePageMakerAttribute
    {
        public PostPageMakerAttribute(PageStyle destUrl)
        {
            DataType = ContentDataType.Json;
            DestUrl = destUrl;
            UseRetUrlFirst = true;
        }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            CustomUrlConfig config = new CustomUrlConfig(false, true, CustomUrl)
            {
                UseKeyData = UseKeyData
            };
            PostPageMaker result = new PostPageMaker(DataType, DestUrl, config)
            {
                UseRetUrlFirst = UseRetUrlFirst,
                AlertMessage = AlertMessage
            };
            return result;
        }

        public ContentDataType DataType { get; set; }

        public PageStyle DestUrl { get; private set; }

        public string CustomUrl { get; set; }

        public bool UseKeyData { get; set; }

        public bool UseRetUrlFirst { get; set; }

        public string AlertMessage { get; set; }
    }
}