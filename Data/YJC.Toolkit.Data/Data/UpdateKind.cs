using System;

namespace YJC.Toolkit.Data
{
    /// <remarks>Enum<c>UpdateKind</c>：数据更新类型的标识</remarks>
    /// <summary>数据更新类型的标识</summary>
    [Flags]
    public enum UpdateKind
    {
        /// <summary>
        /// 插入
        /// </summary>
        Insert = 1,
        /// <summary>
        /// 更新
        /// </summary>
        Update = 2,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 4,

        All = Insert | Update | Delete
    };
}
