using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class CompositeDataRight : IDataRight
    {
        private readonly Dictionary<PageStyleClass, CompositeDataRightItem> fDictionary;

        public CompositeDataRight()
        {
            fDictionary = new Dictionary<PageStyleClass, CompositeDataRightItem>();
        }

        #region IDataRight 成员

        public IParamBuilder GetListSql(ListDataRightEventArgs e)
        {
            PageStyleClass style = (PageStyleClass)PageStyle.List;
            CompositeDataRightItem item;
            if (fDictionary.TryGetValue(style, out item))
                return item.DataRight.CreateObject(e.FieldIndexer).GetListSql(e);
            else
            {
                if (HasRightIfNoItem)
                    return null;
                else
                    return SqlParamBuilder.NoResult;
            }
        }

        public void Check(DataRightEventArgs e)
        {
            PageStyleClass style = PageStyleClass.FromStyle(e.Style);
            CompositeDataRightItem item;
            if (fDictionary.TryGetValue(style, out item))
                item.DataRight.CreateObject(e.FieldIndexer).Check(e);
            else
            {
                if (!HasRightIfNoItem)
                    throw new NoDataRightException();
            }
        }

        #endregion

        public bool HasRightIfNoItem { get; set; }

        internal void Add(CompositeDataRightItem item)
        {
            fDictionary.Add(item.Style, item);
        }

        public void Add(IPageStyle style, IConfigCreator<IDataRight> dataRight)
        {
            PageStyleClass pageStyle = PageStyleClass.FromStyle(style);
            Add(new CompositeDataRightItem(pageStyle, dataRight));
        }
    }
}
