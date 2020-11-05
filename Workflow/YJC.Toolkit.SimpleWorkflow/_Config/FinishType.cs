using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    /// <summary>
    /// 在WfIsEnd中写入的值
    /// </summary>
    [EnumCodeTable(UseIntValue = true)]
    internal enum FinishType
    {
        /// <summary>
        /// 未结束
        /// </summary>
        [DisplayName("未结束")]
        None = 0,

        /// <summary>
        /// 正常结束
        /// </summary>
        [DisplayName("正常结束")]
        Normal = 1,

        /// <summary>
        /// 正常结束，允许对原纪录进行修改（注意，不许删除该记录）
        /// </summary>
        [DisplayName("正常结束")]
        ModifiedNormal,

        /// <summary>
        /// 正常结束，允许再次发起流程，但无法修改
        /// </summary>
        [DisplayName("正常结束")]
        ReUse,

        /// <summary>
        /// 调用终止流程后的状态
        /// </summary>
        [DisplayName("终止")]
        Abort,

        /// <summary>
        /// 因为意外错误，导致流程结束
        /// </summary>
        [DisplayName("错误结束")]
        Error,

        /// <summary>
        /// 错误后，不断重试，超过重试次数后终止
        /// </summary>
        [DisplayName("超过重试次数")]
        OverTryTimes,

        /// <summary>
        /// 配置错误，返回起始步骤，导致终止
        /// </summary>
        [DisplayName("返回起始步骤")]
        ReturnBegin
    }
}