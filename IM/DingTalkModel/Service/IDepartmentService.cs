using System.Collections.Generic;
using YJC.Toolkit.DingTalk.Model.Company;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IDepartmentService
    {
        //获取子部门ID列表
        [ApiMethod("/department/list_ids", ResultKey = "sub_dept_id_list", IsMultiple = true,
            ResultType = ResultType.Simple)]
        List<int> GetDepartmentListIds([ApiParameter]string id);

        // 获取部门列表
        [ApiMethod("/department/list", IsMultiple = true, ResultKey = "department")]
        List<SimpleDepartment> GetDepartmentList([ApiParameter]string id = null,
            [ApiParameter(NamingRule = NamingRule.UnderLineLower)]bool fetchChild = false);

        //获取部门详情
        [ApiMethod("/department/get")]
        Department GetDepartment([ApiParameter]string id);

        // 查询部门的所有上级父部门路径
        [ApiMethod("/department/list_parent_depts_by_dept", ResultKey = "parentIds",
            IsMultiple = true, ResultType = ResultType.Simple)]
        List<int> GetParentDeptIds([ApiParameter]string id);

        //查询指定用户的所有上级父部门路径
        // 做不了，不支持
        //[ApiMethod("/department/list_parent_depts", ResultKey = "department", IsMultiple = true,
        //    ResultType = ResultType.Simple)]
        //List<List<int>> GetParentDeptsByUser([ApiParameter]string userId);

        // 获取企业员工人数
        [ApiMethod("/user/get_org_user_count", ResultKey = "count")]
        int GetUserCount([TkTypeConverter(typeof(BoolIntConverter))][ApiParameter]bool onlyActive = true);

        // 创建部门
        [ApiMethod("/department/create", Method = HttpMethod.Post, ResultKey = "id")]
        string CreateDepartment([ApiParameter(Location = ParamLocation.Content)]Department dept);

        // 更新部门
        [ApiMethod("/department/update", Method = HttpMethod.Post, ResultKey = "id")]
        string UpdateDepartment([ApiParameter(Location = ParamLocation.Content)]Department dept);

        // 删除部门
        [ApiMethod("/department/delete")]
        BaseResult DeleteDepartment([ApiParameter]string id);
    }
}