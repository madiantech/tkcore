using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.DingTalk.Model;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IUserNoticeService
    {
        //获取用户公告数据
        [ApiMethod("/topapi/blackboard/listtopten", Method = HttpMethod.Post,
            ResultKey = "blackboard_list", IsMultiple = true,
            PostType = typeof(IdParam), PostModelName = NameModelConst.USER)]
        List<Notice> GetUserNoticeData([ApiParameter(Location = ParamLocation.Partial)]string userId);
    }
}