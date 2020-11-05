using System.Collections.Generic;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    public interface IRole : IEntity
    {
        /// <summary>
        /// 角色代码
        /// </summary>
        string RoleCode { get; }

        IEnumerable<IUser> Users { get; }
    }
}