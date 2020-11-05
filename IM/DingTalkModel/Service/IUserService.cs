using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.DingTalk.Model;
using YJC.Toolkit.IM;
using YJC.Toolkit.DingTalk.Model.Company;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IUserService
    {
        //获取用户详情
        [ApiMethod("/user/get")]
        User GetUserDetail([ApiParameter(NamingRule = NamingRule.Lower)]string userId);

        //获取部门用户userid列表
        [ApiMethod("/user/getDeptMember", IsMultiple = true, ResultKey = "userIds",
            ResultType = ResultType.Simple)]
        List<int> GetDeptMember([ApiParameter]string deptId);

        //获取部门用户
        [ApiMethod("/user/simplelist")]
        PageResult<SimpleUser> GetSimpleUserList(
            [ApiParameter(NamingRule = NamingRule.UnderLineLower)]long departmentId,
            [ApiParameter]long offset = 0, [ApiParameter]int size = 100,
            [TkTypeConverter(typeof(EnumFieldValueTypeConverter), UseObjectType = true)]
            [ApiParameter]UserOrder order = UserOrder.Custom);

        //获取部门用户详情
        [ApiMethod("/user/listbypage")]
        PageResult<User> GetUserList(
            [ApiParameter(NamingRule = NamingRule.UnderLineLower)]long departmentId,
            [ApiParameter]long offset = 0, [ApiParameter]int size = 100,
            [TkTypeConverter(typeof(EnumFieldValueTypeConverter), UseObjectType = true)]
            [ApiParameter]UserOrder order = UserOrder.Custom);

        //获取管理员列表
        [ApiMethod("/user/get_admin", IsMultiple = true, ResultKey = "admin_list")]
        List<Administrator> GetAdminList();

        //获取管理员通讯录权限范围
        [ApiMethod("/topapi/user/get_admin_scope", ResultKey = "dept_ids", ResultType = ResultType.Simple,
            IsMultiple = true)]
        List<int> GetAdminScope([ApiParameter(NamingRule = NamingRule.Lower)]string userId);

        // 根据unionid获取userid
        [ApiMethod("/user/getUseridByUnionid")]
        UserIdResult GetUserIdByUnionId([ApiParameter(NamingRule = NamingRule.Lower)]string unionId);

        // 创建用户
        [ApiMethod("/user/create", Method = HttpMethod.Post, ResultKey = "userid")]
        string CreateUser([ApiParameter(Location = ParamLocation.Content)]User user);

        // 更新用户
        [ApiMethod("/user/update", Method = HttpMethod.Post)]
        BaseResult UpdateUser([ApiParameter(Location = ParamLocation.Content)]User user);

        // 删除用户
        [ApiMethod("/user/delete")]
        BaseResult DeleteUser([ApiParameter(NamingRule = NamingRule.Lower)]string userId);

        ////查询管理员是否具备管理某个应用的权限*
        //[ApiMethod("/user/can_access_microapp", ResultKey = "canAccess")]
        //bool CanAccessMicroapp([ApiParameter]string appId, [ApiParameter]string userId);
    }
}