using System;

namespace YJC.Toolkit.Data
{
    [Flags]
    public enum RelationType
    {
        /// <summary>
        /// 填充从表记录
        /// </summary>
        OnlyFill = 0x01,
        /// <summary>
        /// 给从表外键赋值
        /// </summary>
        MasterValue = 0x02,
        /// <summary>
        /// 填充从表记录并给其外键赋值值
        /// </summary>
        MasterRelation = 0x03,
        /// <summary>
        /// 给逻辑上从表外键赋值
        /// </summary>
        DetailValue = 0x04,
        /// <summary>
        /// 给逻辑上从表填充记录并给其外键赋值值
        /// </summary>
        DetailRelation = 0x05
    }
}
