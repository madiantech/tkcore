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
    public interface IUserReportService
    {
        //获取用户日志数据
        [ApiMethod("/topapi/report/list", Method = HttpMethod.Post)]
        ReportResult GetUserReport([ApiParameter(Location = ParamLocation.Content)]ReportParam pram);

        //获取用户可见的日志模板
        [ApiMethod("/topapi/report/template/listbyuserid", Method = HttpMethod.Post,
            ResultKey = "result", PostType = typeof(UserListParam))]
        TemplateReportResult GetTemplateReport(
            [ApiParameter(Location = ParamLocation.Partial)]string userId,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0,
            [ApiParameter(Location = ParamLocation.Partial)]int size = UserListParam.MAX);

        //获取用户日志未读数
        [ApiMethod("/topapi/report/getunreadcount", Method = HttpMethod.Post, ResultKey = "count",
            PostType = typeof(IdParam), PostModelName = NameModelConst.USER)]
        int GetUnReadCount([ApiParameter(Location = ParamLocation.Partial)]string userId);
    }
}