using System.Collections;

namespace YJC.Toolkit.Sys
{
    public interface IUserInfo
    {
        string UserName { get; }

        string LogOnName { get; }

        //string Encoding { get; }

        object UserId { get; }

        object MainOrgId { get; }

        IEnumerable OtherOrgs { get; }

        IEnumerable RoleIds { get; }

        int PageSize { get; }

        bool IsLogOn { get; }

        object Data1 { get; set; }

        object Data2 { get; set; }
    }
}
