using System.Collections.Generic;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    public interface IOrganization : IEntity, ITreeNode
    {
        /// <summary>
        /// 组织代码
        /// </summary>
        string OrgCode { get; }

        /// <summary>
        /// 是否有部门
        /// </summary>
        bool HasDepartment { get; }

        /// <summary>
        /// 是否是部门
        /// </summary>
        bool IsDepartment { get; }

        IEnumerable<IUser> Users { get; }
    }
}