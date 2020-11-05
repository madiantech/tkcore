using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public enum StatusList
    {
        Stateless = -1,      //无状态

        ProbationPeriod = 2, //试用期

        Formal = 3,          //正式

        LeavingOffice = 5,   //待离职
    }
}