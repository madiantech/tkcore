namespace YJC.Toolkit.SimpleWorkflow
{
    /// <summary>
    /// 控制Content显示的内容
    /// </summary>
    public enum ProcessDisplay
    {
        /// <summary>
        /// 不显示步骤数据
        /// </summary>
        None,

        /// <summary>
        /// 正常显示
        /// </summary>
        Normal,

        /// <summary>
        /// 显示子流程的数据
        /// </summary>
        Child,

        /// <summary>
        /// 显示父流程的数据
        /// </summary>
        Parent
    }
}