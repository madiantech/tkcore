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
    public interface IWorkRecordService
    {
        //发起待办
        [ApiMethod("/topapi/workrecord/add", Method = HttpMethod.Post, ResultKey = "record_id")]
        string AddWorkrecord([ApiParameter(Location = ParamLocation.Content)]WorkRecord workrecord);

        //更新待办
        [ApiMethod("/topapi/workrecord/update", Method = HttpMethod.Post, ResultKey = "result",
            PostType = typeof(WorkRecordNew))]
        bool UpdateWorkRecord([ApiParameter(Location = ParamLocation.Partial)]string userId,
                              [ApiParameter(Location = ParamLocation.Partial)]string recordId);

        //获取用户待办事项
        [ApiMethod("/topapi/workrecord/getbyuserid", ResultKey = "records",
            Method = HttpMethod.Post, PostType = typeof(WorkRecordParam))]
        PageResult<WorkRecord> GetWorkRecordListByUid(
            [ApiParameter(Location = ParamLocation.Partial)]string userId,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0,
            [ApiParameter(Location = ParamLocation.Partial)]int limit = WorkRecordParam.LIMITMAX,
            [ApiParameter(Location = ParamLocation.Partial)]bool status = false);
    }
}