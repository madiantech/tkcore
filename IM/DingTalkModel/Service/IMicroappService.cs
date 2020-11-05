using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.DingTalk.Model.App;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IMicroappService
    {
        //获取应用列表*
        [ApiMethod("/microapp/list", Method = HttpMethod.Post, IsMultiple = true, ResultKey = "appList")]
        List<AppDetail> GetMicroappList();

        //获取员工可见的应用列表
        [ApiMethod("/microapp/list_by_userid", ResultKey = "appList", IsMultiple = true)]
        List<AppDetail> GetListByUserid([ApiParameter]string Userid);

        //获取应用的可见范围
        [ApiMethod("/microapp/visible_scopes", Method = HttpMethod.Post, PostType = typeof(VisibleScopesParam))]
        VisibleScopesResult GetVisibleScopes(
            [ApiParameter(Location = ParamLocation.Partial)]long agentId);

        //设置应用的可见范围
        [ApiMethod("/microapp/set_visible_scopes", Method = HttpMethod.Post)]
        BaseResult SetVisibleScopes([ApiParameter(Location = ParamLocation.Content)]VisibleScopesResult pram);
    }
}