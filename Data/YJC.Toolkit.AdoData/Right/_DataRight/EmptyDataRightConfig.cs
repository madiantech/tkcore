using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [DataRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-28", Description = "空数据权限")]
    internal class EmptyDataRightConfig : IConfigCreator<IDataRight>
    {
        #region IConfigCreator<IDataRight> 成员

        public IDataRight CreateObject(params object[] args)
        {
            return new EmptyDataRight(AllowAll).SetErrorText(ErrorMessage);
        }

        #endregion

        [SimpleAttribute]
        public bool AllowAll { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText ErrorMessage { get; protected set; }
    }
}
