using System.Collections.Generic;

namespace YJC.Toolkit.Data
{
    public interface ITreeNode : IEntity
    {
        /// <summary>
        /// 是否有子节点
        /// </summary>
        bool HasChildren { get; }

        /// <summary>
        /// 是否有父节点
        /// </summary>
        bool HasParent { get; }

        /// <summary>
        /// 获取父节点的Id
        /// </summary>
        string ParentId { get; }

        /// <summary>
        /// 获取父节点
        /// </summary>
        /// <returns>父节点对象</returns>
        ITreeNode Parent { get; }

        /// <summary>
        /// 获取所有的子节点对象
        /// </summary>
        /// <returns>子节点对象集合</returns>
        IEnumerable<ITreeNode> Children { get; }
    }
}
