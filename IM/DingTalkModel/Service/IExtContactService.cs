using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.DingTalk.Model;
using YJC.Toolkit.DingTalk.Model.Company;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IExtContactService
    {
        //获取外部联系人标签列表
        [ApiMethod("/topapi/extcontact/listlabelgroups", Method = HttpMethod.Post,
            PostType = typeof(ListParam), IsMultiple = true, ResultKey = "results")]
        List<ExtContactLabelList> GetExtLabelList(
            [ApiParameter(Location = ParamLocation.Partial)]int size = ListParam.DEFAULT,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0);

        //获取外部联系人列表
        [ApiMethod("/topapi/extcontact/list", Method = HttpMethod.Post, ResultKey = "results",
            IsMultiple = true, PostType = typeof(ListParam))]
        List<ExtContact> GetExtList(
            [ApiParameter(Location = ParamLocation.Partial)]int size = ListParam.DEFAULT,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0);

        //获取企业外部联系人详情
        [ApiMethod("/topapi/extcontact/get", Method = HttpMethod.Post, ResultKey = "result",
            PostType = typeof(ExtDetailParam))]
        ExtContact GetExtcontact([ApiParameter(Location = ParamLocation.Partial)]string userId);

        //添加外部联系人
        [ApiMethod("/topapi/extcontact/create", Method = HttpMethod.Post, ResultKey = "userid")]
        string CreateExtcontact([ApiParameter(Location = ParamLocation.Content)]Contact contacts);

        //更新外部联系人
        [ApiMethod("/topapi/extcontact/update", Method = HttpMethod.Post)]
        BaseResult UpdateExtcontact([ApiParameter(Location = ParamLocation.Content)]ContactUpdate contacts);

        //删除外部联系人
        [ApiMethod("/topapi/extcontact/delete", Method = HttpMethod.Post, PostType = typeof(ExtDetailParam))]
        BaseResult DeleteExtcontact([ApiParameter(Location = ParamLocation.Partial)]string userId);
    }
}