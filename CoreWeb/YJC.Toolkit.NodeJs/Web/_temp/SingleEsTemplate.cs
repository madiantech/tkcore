using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [EsTemplate]
    internal class SingleEsTemplate : BaseEsTemplate
    {
        public SingleEsTemplate()
            : base("single", "insert", "update", "detail", "list", "index")
        {
        }
    }
}