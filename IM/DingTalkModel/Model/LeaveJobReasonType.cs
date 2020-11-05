using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.DingTalk.Model
{
    public enum LeaveJobReason
    {
        //离职原因类型：1，家庭原因；2，个人原因；3，发展原因；4，合同到期不续签；
        //5，协议解除；6，无法胜任工作；7，经济性裁员；8，严重违法违纪；9，其他

        Family = 1,
        Oneself = 2,
        Approve = 3,
        NotRenew = 4,
        DissolutionOfAgreement = 5,
        UncompetentForWork = 6,
        EconomicLayoffs = 7,
        SeriousViolations = 8,
        Other = 9
    }
}