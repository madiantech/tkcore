using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class TableMetaDataConfig : DetailSingleMetaDataConfig
    {
        [SimpleAttribute(Required = true)]
        public PageStyle Action { get; protected set; }
    }
}