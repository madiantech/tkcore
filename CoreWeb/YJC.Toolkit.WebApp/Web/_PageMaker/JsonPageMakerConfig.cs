using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "复合Object和DataSet，都能输出为Json",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-17")]
    internal class JsonPageMakerConfig : BaseObjectConfig
    {
        public override IPageMaker CreateObject(params object[] args)
        {
            return new JsonPageMaker(this);
        }
    }
}