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
    public interface IProcessService
    {
        //发起审批实例
        [ApiMethod("/topapi/processinstance/create", Method = HttpMethod.Post,
            ResultKey = "process_instance_id")]
        string CreateProcess([ApiParameter(Location = ParamLocation.Content)]ProcessDetail detail);

        //批量获取审批实例id
        [ApiMethod("/topapi/processinstance/listids", Method = HttpMethod.Post,
            ResultKey = "result")]
        ListIdsResult GetProcessIds(
            [ApiParameter(Location = ParamLocation.Content)]ProcessInstanceParam pram);

        //获取单个审批实例
        [ApiMethod("/topapi/processinstance/get", Method = HttpMethod.Post, ResultKey = "process_instance",
            PostType = typeof(IdParam), PostModelName = NameModelConst.PROCESS_INSTANCE_ID)]
        ProcessInstanceResult GetProcessInstance(
            [ApiParameter(Location = ParamLocation.Partial)]string processInstanceId);

        //获取用户待审批数量
        [ApiMethod("/topapi/process/gettodonum", Method = HttpMethod.Post, ResultKey = "count",
            PostType = typeof(IdParam), PostModelName = NameModelConst.USER)]
        int GetToDoNum([ApiParameter(Location = ParamLocation.Partial)]string userId);

        //获取用户可见的审批模板
        [ApiMethod("/topapi/process/listbyuserid", Method = HttpMethod.Post, ResultKey = "result",
            PostType = typeof(UserListParam))]
        ProcessTemplateResult GetProcessTempByUid(
            [ApiParameter(Location = ParamLocation.Partial)]string userId,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0,
            [ApiParameter(Location = ParamLocation.Partial)]int size = UserListParam.MAX);
    }
}