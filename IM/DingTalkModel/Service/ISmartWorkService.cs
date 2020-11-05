using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.DingTalk.Model;
using YJC.Toolkit.DingTalk.Model.Office;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface ISmartWorkService
    {
        //获取员工花名册字段信息*附录Platform System error
        [ApiMethod("/topapi/smartwork/hrm/employee/list", Method = HttpMethod.Post,
            PostType = typeof(EmployeeParam))]
        EmployeeResult GetEmployeeInfoList(
            [ApiParameter(Location = ParamLocation.Partial)]string userIdList,
            [ApiParameter(Location = ParamLocation.Partial)]string fieldFilterList);

        //查询企业待入职员工列表
        [ApiMethod("/topapi/smartwork/hrm/employee/querypreentry", Method = HttpMethod.Post,
            PostType = typeof(ListParam))]
        EmployeeListResult GetWaitJoinEmpList(
            [ApiParameter(Location = ParamLocation.Partial)]int size = 50,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0);

        //查询企业在职员工列表
        [ApiMethod("/topapi/smartwork/hrm/employee/queryonjob", Method = HttpMethod.Post,
            PostType = typeof(EmpListParam))]
        EmployeeListResult GetWorkingEmpList(
            [ApiParameter(Location = ParamLocation.Partial)]StatusList statusList,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0,
            [ApiParameter(Location = ParamLocation.Partial)]int size = 20);

        //查询企业离职员工列表
        [ApiMethod("/topapi/smartwork/hrm/employee/querydimission", Method = HttpMethod.Post,
            PostType = typeof(ListParam))]
        EmployeeListResult GetLeaveJobEmpList(
            [ApiParameter(Location = ParamLocation.Partial)]int size = 50,
            [ApiParameter(Location = ParamLocation.Partial)]int offset = 0);

        //获取离职员工离职信息*userid_list
        [ApiMethod("/topapi/smartwork/hrm/employee/listdimission", Method = HttpMethod.Post,
            PostType = typeof(EmployeeParam))]
        EmpLeaveJobResult GetLeaveJobEmpInfo(
            [ApiParameter(Location = ParamLocation.Partial)]string userIdList);

        //添加企业待入职员工
        [ApiMethod("/topapi/smartwork/hrm/employee/addpreentry", Method = HttpMethod.Post)]
        EmployeeDetailResult AddWaitEmployee(
            [ApiParameter(Location = ParamLocation.Content)]EmployeeDetailParam param);
    }
}