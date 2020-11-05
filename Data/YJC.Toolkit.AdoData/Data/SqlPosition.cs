using System;

namespace YJC.Toolkit.Data
{
    /// <remarks>Enum<c>SQLFlag</c>：SQL语句生成标记</remarks>
    /// <summary>SQL语句生成标记</summary>
    [Flags]
    public enum SqlPosition
    {
        /// <summary>
        /// 空
        /// </summary>
        None = 0,
        /// <summary>
        /// 数据更新
        /// </summary>
        Update = 1,
        /// <summary>
        /// 条件子句
        /// </summary>
        Where = 2
    }
}
