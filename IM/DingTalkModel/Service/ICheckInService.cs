using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.DingTalk.Model;
using YJC.Toolkit.DingTalk.Model.Office;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface ICheckinService
    {
        //获取部门用户签到记录
        [ApiMethod("/checkin/record", IsMultiple = true, ResultKey = "data")]
        List<CheckinRecord> GetDeptCheckinRecord(
           [ApiParameter(NamingRule = NamingRule.UnderLineLower)]string departmentId,
           [ApiParameter(NamingRule = NamingRule.UnderLineLower)][TkTypeConverter(typeof(IMDateTimeConverter))]long startTime,
           [ApiParameter(NamingRule = NamingRule.UnderLineLower)][TkTypeConverter(typeof(IMDateTimeConverter))]long endTime,
           [ApiParameter]long offset, [ApiParameter]int size,
           [ApiParameter][TkTypeConverter(typeof(LowerCaseEnumConverter), UseObjectType = true)]Order order);

        //获取用户签到记录
        [ApiMethod("/topapi/checkin/record/get", Method = HttpMethod.Post, ResultKey = "result")]
        UserCheckinResult GetUserChekinRecord(
            [ApiParameter(Location = ParamLocation.Content)]UserCheckinRecord record);
    }
}