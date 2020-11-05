using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public enum SourceType
    {
        [EnumFieldValue("ATM")]
        Atm,

        [EnumFieldValue("BEACON")]
        Beacon,

        [EnumFieldValue("DING_ATM")]
        DingAtm,

        [EnumFieldValue("USER")]
        User,

        [EnumFieldValue("BOSS")]
        Boss,

        [EnumFieldValue("APPROVE")]
        Approve,

        [EnumFieldValue("SYSTEM")]
        System,

        [EnumFieldValue("AUTO_CHECK")]
        AutoCheck
    }
}