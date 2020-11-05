using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeCorp.Model;

namespace YJC.Toolkit.WeCorp.Service
{
    public interface IBatchSyncService
    {
        //增量更新成员
        [ApiMethod("/batch/syncuser", Method = HttpMethod.Post, ResultKey = "jobid")]
        string BatchUpdateUser([ApiParameter(Location = ParamLocation.Content)]SyncUser user);

        //全量覆盖成员
        [ApiMethod("/batch/replaceuser", Method = HttpMethod.Post, ResultKey = "jobid")]
        string BatchReplaceUser([ApiParameter(Location = ParamLocation.Content)]SyncUser user);

        //全量覆盖部门
        [ApiMethod("/batch/replaceparty", Method = HttpMethod.Post, ResultKey = "jobid")]
        string BatchReplaceParty([ApiParameter(Location = ParamLocation.Content)]SyncUser user);

        //获取异步任务结果
        [ApiMethod("/batch/getresult")]
        SyncResult GetBatchResult([ApiParameter(NamingRule = NamingRule.Lower)]string jobId);
    }
}