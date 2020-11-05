using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public abstract class BaseFieldRightConfig : IConfigCreator<IDataRight>
    {
        protected BaseFieldRightConfig()
        {
        }

        #region IConfigCreator<IDataRight> 成员

        public IDataRight CreateObject(params object[] args)
        {
            IFieldInfoIndexer indexer = ObjectUtil.ConfirmQueryObject<IFieldInfoIndexer>(this, args);

            IFieldInfo ownerField = indexer[NickName];
            TkDebug.AssertNotNull(ownerField, string.Format(ObjectUtil.SysCulture,
                "没有找到昵称为{0}的字段", NickName), indexer);
            return CreateDataRight(ownerField).SetErrorText(ErrorMessage);
        }

        #endregion

        protected abstract IDataRight CreateDataRight(IFieldInfo fieldInfo);

        [SimpleAttribute]
        public string NickName { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText ErrorMessage { get; protected set; }
    }
}
