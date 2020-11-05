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
    public interface IRoleService
    {
        //获取角色列表
        [ApiMethod("/topapi/role/list", Method = HttpMethod.Post, PostType = typeof(ListParam),
            ResultKey = "result")]
        PageResult<RoleListDetail> GetRoleList(
            [ApiParameter(Location = ParamLocation.Partial)]int size = ListParam.DEFAULT,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0);

        //获取角色下的员工列表
        [ApiMethod("/topapi/role/simplelist", Method = HttpMethod.Post, PostType = typeof(RoleListParam),
            ResultKey = "result")]
        PageResult<SimpleUser> GetRoleSimpleList(
            [ApiParameter(Location = ParamLocation.Partial)]int roleId,
            [ApiParameter(Location = ParamLocation.Partial)]int size = ListParam.DEFAULT,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0);

        //获取角色组
        [ApiMethod("/topapi/role/getrolegroup", Method = HttpMethod.Post, ResultKey = "role_group",
            PostType = typeof(IdParam), PostModelName = NameModelConst.ROLE_GROUP)]
        RoleGroupResult GetRoleGroup([ApiParameter(Location = ParamLocation.Partial)]int groupId);

        //获取角色详情
        [ApiMethod("/topapi/role/getrole", Method = HttpMethod.Post, ResultKey = "role",
            PostType = typeof(IdParam), PostModelName = NameModelConst.ROLE)]
        RoleInfo GetRoleDetail([ApiParameter(Location = ParamLocation.Partial)]int roleId);

        //创建角色
        [ApiMethod("/role/add_role", Method = HttpMethod.Post, ResultKey = "roleId",
            PostType = typeof(RoleNew))]
        int AddRole([ApiParameter(Location = ParamLocation.Partial)]string roleName,
                    [ApiParameter(Location = ParamLocation.Partial)]int groupId);

        //更新角色
        [ApiMethod("/role/update_role", Method = HttpMethod.Post, PostType = typeof(Role))]
        BaseResult UpdateRole([ApiParameter(Location = ParamLocation.Partial)]string roleName,
                              [ApiParameter(Location = ParamLocation.Partial)]int roleId);

        //删除角色
        [ApiMethod("/topapi/role/deleterole", Method = HttpMethod.Post, PostType = typeof(IdParam),
            PostModelName = NameModelConst.ROLE_ID)]
        BaseResult DeleteRole([ApiParameter(Location = ParamLocation.Partial)]int roleId);

        //创建角色组
        [ApiMethod("/role/add_role_group", Method = HttpMethod.Post, ResultKey = "groupId",
            PostType = typeof(NameParam), PostModelName = NameModelConst.NAME)]
        int AddRoleGroup([ApiParameter(Location = ParamLocation.Partial)]string name);

        //批量增加员工角色
        [ApiMethod("/topapi/role/addrolesforemps", Method = HttpMethod.Post, PostType = typeof(RolesParam))]
        BaseResult AddRoles([ApiParameter(Location = ParamLocation.Partial)]string roleIds,
                            [ApiParameter(Location = ParamLocation.Partial)]string userIds);

        //批量删除员工角色
        [ApiMethod("/topapi/role/removerolesforemps", Method = HttpMethod.Post, PostType = typeof(RolesParam))]
        BaseResult RemoveRoles([ApiParameter(Location = ParamLocation.Partial)]string roleIds,
                               [ApiParameter(Location = ParamLocation.Partial)]string userIds);
    }
}