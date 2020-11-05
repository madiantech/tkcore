namespace YJC.Toolkit.Sys
{
    /// <remarks>接口<c>IAuthor</c>：开发人员信息接口
    /// </remarks>
    /// <summary>开发人员信息接口</summary>
    public interface IAuthor
    {
        /// <value>属性<c>Author</c>：开发人员
        /// </value>
        /// <summary>开发人员</summary>
        string Author { get; }

        /// <value>属性<c>Author</c>：功能描述
        /// </value>
        /// <summary>功能描述</summary>
        string Description { get; }

        /// <value>属性<c>CreateDate</c>：创建时间
        /// </value>
        /// <summary>创建时间</summary>
        string CreateDate { get; }
    }
}
