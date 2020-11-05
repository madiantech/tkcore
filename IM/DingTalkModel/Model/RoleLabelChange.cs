using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class RoleLabelChange
    {
        [SimpleElement(LocalName = "roleLabelChange")]
        public List<string> RoleLabelChanges { get; set; }
    }
}