using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [EsTemplate]
    internal class MultipleEsTemplate : BaseEsTemplate
    {
        public MultipleEsTemplate()
            : base("single", "multiinsert", "multiupdate", "multidetail", "list", "multiindex")
        {
        }
    }
}