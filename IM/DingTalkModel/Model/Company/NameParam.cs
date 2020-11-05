using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    internal class NameParam
    {
        public NameParam(string name)
        {
            Name = name;
        }

        [SimpleElement]
        [NameModel(NameModelConst.NAME, LocalName = "name")]
        public string Name { get; set; }
    }
}