using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web._temp
{
    [EsTemplate]
    internal class SingleTreeEsTemplate : BaseEsTemplate
    {
        public SingleTreeEsTemplate()
         : base("single", "treeinsert", "treeupdate", "treedetail", "tree", "index")
        {
        }
    }
}