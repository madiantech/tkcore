using System;

namespace YJC.Toolkit.Data
{
    /// <remarks>Enum<c>AdapterCommand</c>：数据库适配器的数据库命令标记</remarks>
    /// <summary>数据库适配器的数据库命令标记</summary>
    [Flags]
    public enum AdapterCommand
    {
        /// <summary>
        /// Insert命令
        /// </summary>
        Insert = 1,
        /// <summary>
        /// Update命令
        /// </summary>
        Update = 2,
        /// <summary>
        /// Delete命令
        /// </summary>
        Delete = 4,
        /// <summary>
        /// Select命令
        /// </summary>
        Select = 8,

        All = Insert | Update | Delete
    };
}
