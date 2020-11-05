using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.WeCorp.Model.Company;

namespace YJC.Toolkit.WeCorp.Service
{
    public interface IDepartmentService
    {
        [ApiMethod("/department/list", IsMultiple = true, ResultKey = "department", UseConstructor = true)]
        List<Department> GetDepartmentList([ApiParameter]int? id = null);

        [ApiMethod("/department/create", Method = HttpMethod.Post, ResultKey = "id")]
        int CreateDepartment([ApiParameter(Location = ParamLocation.Content)]Department dept);

        [ApiMethod("/department/delete")]
        BaseResult DeleteDepartment([ApiParameter]int id);

        [ApiMethod("/department/update", Method = HttpMethod.Post)]
        BaseResult UpdateDepartment(
            [ApiParameter(Location = ParamLocation.Content)]Department dept);
    }
}