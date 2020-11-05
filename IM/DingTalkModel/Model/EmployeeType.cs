using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.DingTalk.Model
{
    public enum EmployeeType
    {
        //员工类型枚举值（0，无类型;1，全职;2，兼职;3，实习;4，劳务派遣;5，退休返聘;6，劳务外包）;
        No = 0,

        FullTime = 1,

        PartTime = 2,

        Internship = 3,

        LaborDispatch = 4,

        RetirementRecruitment = 5,

        Outsourcing = 6
    }
}