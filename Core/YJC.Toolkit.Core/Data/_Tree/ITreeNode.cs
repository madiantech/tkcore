using System.Collections.Generic;

namespace YJC.Toolkit.Data
{
    public interface ITreeNode : IEntity
    {
        /// <summary>
        /// �Ƿ����ӽڵ�
        /// </summary>
        bool HasChildren { get; }

        /// <summary>
        /// �Ƿ��и��ڵ�
        /// </summary>
        bool HasParent { get; }

        /// <summary>
        /// ��ȡ���ڵ��Id
        /// </summary>
        string ParentId { get; }

        /// <summary>
        /// ��ȡ���ڵ�
        /// </summary>
        /// <returns>���ڵ����</returns>
        ITreeNode Parent { get; }

        /// <summary>
        /// ��ȡ���е��ӽڵ����
        /// </summary>
        /// <returns>�ӽڵ���󼯺�</returns>
        IEnumerable<ITreeNode> Children { get; }
    }
}
