using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.DingTalk.Model;
using YJC.Toolkit.DingTalk.Model.Office;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IAttendanceService
    {
        //企业考勤排班详情
        [ApiMethod("/topapi/attendance/listschedule", Method = HttpMethod.Post,
            ResultKey = "result", PostType = typeof(NoticeDetailParam))]
        PageResult<SchedulesDetail> GetAttendaceDetail(
            [ApiParameter(Location = ParamLocation.Partial)]DateTime workDate,
            [ApiParameter(Location = ParamLocation.Partial)]int size = ListParam.DEFAULT,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0);

        //企业考勤组详情
        [ApiMethod("/topapi/attendance/getsimplegroups", Method = HttpMethod.Post,
            ResultKey = "result", PostType = typeof(ListParam))]
        PageResult<GroupDetail> GetSimpleGroups(
            [ApiParameter(Location = ParamLocation.Partial)]int size = ListParam.ATTENDANCE,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0);

        //获取打卡详情
        [ApiMethod("/attendance/listRecord", Method = HttpMethod.Post, IsMultiple = true,
            ResultKey = "recordresult")]
        List<ListRecordResult> GetListRecordDetail(
            [ApiParameter(Location = ParamLocation.Content)]ListRecordParam param);

        //获取打卡结果
        [ApiMethod("/attendance/list", Method = HttpMethod.Post)]
        PageResult<RecordResultDetail> GetAttendanceResult(
            [ApiParameter(Location = ParamLocation.Content)]AttendanceParam param);

        //获取请假时长
        [ApiMethod("/topapi/attendance/getleaveapproveduration", Method = HttpMethod.Post,
            ResultKey = "result", PostType = typeof(LeaveApproveParam))]
        LeaveApproveResult GetLeaveTime(
            [ApiParameter(Location = ParamLocation.Partial)]string userId,
            [ApiParameter(Location = ParamLocation.Partial)]DateTime fromDate,
            [ApiParameter(Location = ParamLocation.Partial)]DateTime toDate);

        //查询请假状态
        [ApiMethod("/topapi/attendance/getleavestatus", Method = HttpMethod.Post)]
        LeaveStatusResult GetLeaveStatus(
            [ApiParameter(Location = ParamLocation.Content)]LeaveStatusParam param);

        //获取用户考勤组
        [ApiMethod("/topapi/attendance/getusergroup", Method = HttpMethod.Post,
            ResultKey = "result", PostType = typeof(IdParam), PostModelName = NameModelConst.USER)]
        UserGroupResult GetUserGroup(
            [ApiParameter(Location = ParamLocation.Partial)]string userId);
    }
}