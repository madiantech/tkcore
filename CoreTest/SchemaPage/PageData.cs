using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite
{
    public class PageData
    {
        public PageData()
        {
            Groups = new List<SectionGroup>();
        }

        public List<SectionGroup> Groups { get; }
    }
}