using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    internal class CompositeDataRightItem
    {
        public CompositeDataRightItem()
        {
        }

        public CompositeDataRightItem(PageStyleClass style, IConfigCreator<IDataRight> dataRight)
        {
            Style = style;
            DataRight = dataRight;
        }

        [SimpleAttribute]
        public PageStyleClass Style { get; private set; }

        [DynamicElement(DataRightConfigFactory.REG_NAME)]
        public IConfigCreator<IDataRight> DataRight { get; private set; }
    }
}
