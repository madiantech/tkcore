using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.DingTalk.Model;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface ISportService
    {
        //获取用户钉钉运动开启状态
        [ApiMethod("/topapi/health/stepinfo/getuserstatus", Method = HttpMethod.Post,
            ResultKey = "status", PostType = typeof(IdParam), PostModelName = NameModelConst.USER)]
        bool GetDingSportOpenStatus([ApiParameter(Location = ParamLocation.Partial)]string userId);

        //获取个人或部门的钉钉运动数据
        [ApiMethod("/topapi/health/stepinfo/list", Method = HttpMethod.Post,
            ResultKey = "stepinfo_list", IsMultiple = true, PostType = typeof(StepInfoParam))]
        List<StepInfoResult> GetDingSportsData(
            [ApiParameter(Location = ParamLocation.Partial)]StepType type,
            [ApiParameter(Location = ParamLocation.Partial)]string objectId,
            [ApiParameter(Location = ParamLocation.Partial)]string statDates);

        //批量获取钉钉运动数据
        [ApiMethod("/topapi/health/stepinfo/listbyuserid", Method = HttpMethod.Post,
            ResultKey = "stepinfo_list", IsMultiple = true, PostType = typeof(StepListInfoParam))]
        List<StepInfoResult> GetDingSportsDataList(
            [ApiParameter(Location = ParamLocation.Partial)]string userIds,
            [ApiParameter(Location = ParamLocation.Partial)]string statDate);
    }
}