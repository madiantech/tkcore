namespace YJC.Toolkit.Sys
{
    /// <remarks>�ӿ�<c>IAuthor</c>��������Ա��Ϣ�ӿ�
    /// </remarks>
    /// <summary>������Ա��Ϣ�ӿ�</summary>
    public interface IAuthor
    {
        /// <value>����<c>Author</c>��������Ա
        /// </value>
        /// <summary>������Ա</summary>
        string Author { get; }

        /// <value>����<c>Author</c>����������
        /// </value>
        /// <summary>��������</summary>
        string Description { get; }

        /// <value>����<c>CreateDate</c>������ʱ��
        /// </value>
        /// <summary>����ʱ��</summary>
        string CreateDate { get; }
    }
}
