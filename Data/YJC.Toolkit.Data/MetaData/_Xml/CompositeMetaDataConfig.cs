using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(Description = "根据具体条件来选择相应配置的MetaData", Author = "YJC",
        NamespaceType = NamespaceType.Toolkit, CreateDate = "2013-06-29")]
    [ObjectContext]
    internal class CompositeMetaDataConfig : IConfigCreator<IMetaData>
    {

        #region IConfigCreator<IMetaData> 成员

        public IMetaData CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);

            if (Items != null)
            {
                foreach (CompositeMetaDataItemConfig item in Items)
                {
                    if (item.Condition.UseCondition(input))
                        return item.MetaData.CreateObject(args);
                }
            }

            return null;
        }

        #endregion

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item", Required = true)]
        public List<CompositeMetaDataItemConfig> Items { get; private set; }
    }
}
