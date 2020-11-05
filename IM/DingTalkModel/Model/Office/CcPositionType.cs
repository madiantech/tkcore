using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public enum CcPosition
    {
        [EnumFieldValue("START")]
        Start,

        [EnumFieldValue("FINISH")]
        Finish,

        [EnumFieldValue("START_FINISH")]
        StartFinish
    }
}