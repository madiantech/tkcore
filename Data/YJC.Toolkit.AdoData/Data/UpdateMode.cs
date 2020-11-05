namespace YJC.Toolkit.Data
{
    /// <remarks>Enum<c>UpdateMode</c>：数据更新模式</remarks>
    /// <summary>数据更新模式</summary>
    public enum UpdateMode
    {
        /// <summary>
        /// 单条记录
        /// </summary>
        OneRow,
        /// <summary>
        /// 删除/插入
        /// </summary>
        DelIns,
        /// <summary>
        /// 合并
        /// </summary>
        Merge
    };
}
