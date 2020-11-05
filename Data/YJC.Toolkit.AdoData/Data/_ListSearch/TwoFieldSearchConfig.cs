using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [ListSearchConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-05-10",
        Description = "同一个条件用两个字段进行查询")]
    internal class TwoFieldSearchConfig : IConfigCreator<BaseListSearch>
    {
        #region IConfigCreator<BaseListSearch> 成员

        public BaseListSearch CreateObject(params object[] args)
        {
            IFieldInfoIndexer indexer = ObjectUtil.ConfirmQueryObject<IFieldInfoIndexer>(this, args);
            IFieldInfo current = ObjectUtil.ConfirmQueryObject<IFieldInfo>(this, args);

            IFieldInfo other = indexer[OtherNickName];
            TkDebug.AssertNotNull(other, string.Format(ObjectUtil.SysCulture,
                "{0}不存在，请确认配置是否正确", OtherNickName), this);
            return new TwoFieldListSearch(current, other);
        }

        #endregion IConfigCreator<BaseListSearch> 成员

        [SimpleAttribute]
        public bool WordSplit { get; private set; }

        [SimpleAttribute(Required = true)]
        public string OtherNickName { get; private set; }
    }
}