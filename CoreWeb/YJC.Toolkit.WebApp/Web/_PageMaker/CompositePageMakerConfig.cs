using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "根据具体条件来选择相应配置的PageMaker", 
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-16")]
    internal class CompositePageMakerConfig : IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            IPageData pageData = ObjectUtil.ConfirmQueryObject<IPageData>(this, args);

            CompositePageMaker pageMaker = new CompositePageMaker(pageData);
            if (Items != null)
                foreach (var item in Items)
                {
                    try
                    {
                        IPageMaker itemMaker = item.PageMaker.CreateObject(args);
                        pageMaker.Add(item.Condition.UseCondition, itemMaker);
                    }
                    catch
                    {
                    }
                }

            pageMaker.InternalSetCallInfo(pageData);

            return pageMaker;
        }

        #endregion

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item")]
        public List<CompositePageMakerItemConfig> Items { get; private set; }
    }
}
