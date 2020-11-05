using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.DingTalk.Model
{
    public enum PreStatus
    {
        //离职前工作状态：1，待入职；2，试用期；3，正式
        WaitingForEntry = 1,

        ProbationPeriod = 2,

        Formal = 3
    }
}