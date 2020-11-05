using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeCorp.Model.Application;

namespace YJC.Toolkit.WeCorp.Service
{
    public interface IAppManagementService
    {
        //获取指定的应用详情
        [ApiMethod("/agent/get")]
        AppDetail GetAgent([ApiParameter(NamingRule = NamingRule.Lower)]string agentId);

        //获取access_token对应的应用列表
        [ApiMethod("/agent/list", IsMultiple = true, ResultKey = "agentlist")]
        List<AppDetail> GetAgentList();

        //设置应用
        [ApiMethod("/agent/set")]
        BaseResult SetAgent([ApiParameter(Location = ParamLocation.Content)]AppDetail app);

        //创建菜单
        [ApiMethod("/menu/create", Method = HttpMethod.Post)]
        BaseResult CreateMenu();

        //获取菜单
        [ApiMethod("/menu/get")]
        BaseResult GetMenu([ApiParameter(NamingRule = NamingRule.Lower)]string agentId);

        //删除菜单
        [ApiMethod("/menu/delete")]
        BaseResult DeleteMenu([ApiParameter(NamingRule = NamingRule.Lower)]string agentId);
    }
}