using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class EmployeeDetailParam
    {
        [ObjectElement(LocalName = "param")]
        public EmployeeDetail detail { get; set; }
    }
}