using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class JsonPageMakerConfig : BaseObjectConfig
    {
        public override IPageMaker CreateObject(params object[] args)
        {
            return new JsonPageMaker(this);
        }
    }
}