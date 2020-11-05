using System;

namespace YJC.Toolkit.MetaData
{
    [Flags]
    public enum PageStyle
    {
        /// <summary>
        /// 不在任何页面出现
        /// </summary>
        None = 0,
        /// <summary>
        /// 自定义页面
        /// </summary>
        Custom = 0x20,
        /// <summary>
        /// 数据插入页面
        /// </summary>
        Insert = 0x1,
        /// <summary>
        /// 数据更新页面
        /// </summary>
        Update = 0x2,
        /// <summary>
        /// 数据删除页面
        /// </summary>
        Delete = 0x4,
        /// <summary>
        /// 数据明细页面
        /// </summary>
        Detail = 0x8,
        /// <summary>
        /// 数据列表页面
        /// </summary>
        List = 0x10,

        AllNoList = Insert | Update | Detail,
        All = AllNoList | List

    };
}
