using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeChat.Model.User;

namespace YJC.Toolkit.WeChat.Service
{
    public interface IUserService
    {
        [ApiMethod("/user/get", UseConstructor = true)]
        WeFanContainer GetFanList([ApiParameter(NamingRule = NamingRule.UnderLineLower)]string nextOpenid);

        [ApiMethod("/user/info", UseConstructor = true)]
        WeUser GetUerInfo([ApiParameter]string openid, [ApiParameter]string lang);

        [ApiMethod("/user/info/updateremark", Method = HttpMethod.Post)]
        BaseResult UpdateRemark([ApiParameter(Location = ParamLocation.Content)]WeUserRemark remark);

        [ApiMethod("/user/info/batchget", UseConstructor = true, Method = HttpMethod.Post,
            IsMultiple = true, ResultKey = "user_info_list")]
        List<WeUser> BatchGetUserInfos([ApiParameter(Location = ParamLocation.Content)]BatchOpenIdList list);
    }
}