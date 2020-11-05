using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    public interface IPost : IEntity, ITreeNode
    {
        /// <summary>
        /// 岗位代码
        /// </summary>
        string PostCode { get; }
    }
}