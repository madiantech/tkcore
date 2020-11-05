using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [ListSearchConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-05-10",
        Description = "同一个条件用多个字段进行查询")]
    internal class MultipleFieldSearchConfig : IConfigCreator<BaseListSearch>
    {
        #region IConfigCreator<BaseListSearch> 成员

        public BaseListSearch CreateObject(params object[] args)
        {
            IFieldInfoIndexer indexer = ObjectUtil.ConfirmQueryObject<IFieldInfoIndexer>(this, args);
            IFieldInfo current = ObjectUtil.ConfirmQueryObject<IFieldInfo>(this, args);

            var fields = EnumUtil.Convert(current, GetFieldInfoList(indexer));
            return new MultipleFieldListSearch(fields.ToArray());
        }

        #endregion IConfigCreator<BaseListSearch> 成员

        [SimpleAttribute]
        public bool WordSplit { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "NickName", Required = true)]
        public List<string> NickNameList { get; private set; }

        private IEnumerable<IFieldInfo> GetFieldInfoList(IFieldInfoIndexer indexer)
        {
            foreach (var nickName in NickNameList)
            {
                IFieldInfo field = indexer[nickName];
                TkDebug.AssertNotNull(field, string.Format(ObjectUtil.SysCulture,
                    "{0}不存在，请确认配置是否正确", nickName), this);
                yield return field;
            }
        }
    }
}