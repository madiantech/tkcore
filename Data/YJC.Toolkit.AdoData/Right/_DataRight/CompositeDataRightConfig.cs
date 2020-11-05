using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [DataRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-28", Description = "根据配置选择对应的数据权限")]
    internal class CompositeDataRightConfig : IConfigCreator<IDataRight>
    {
        #region IConfigCreator<IMobileDataRight> 成员

        public IDataRight CreateObject(params object[] args)
        {
            CompositeDataRight dataRight = new CompositeDataRight
            {
                HasRightIfNoItem = HasRightIfNoItem
            };
            if (Items != null)
                foreach (var item in Items)
                    dataRight.Add(item);

            return dataRight;
        }

        #endregion

        [SimpleAttribute]
        public bool HasRightIfNoItem { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "RightItem")]
        public List<CompositeDataRightItem> Items { get; private set; }
    }
}
