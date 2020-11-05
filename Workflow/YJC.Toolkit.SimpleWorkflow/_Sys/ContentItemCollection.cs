using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal sealed class ContentItemCollection : RegNameList<ContentItem>
    {
        internal ContentItemCollection()
        {
        }

        internal ContentItem MainItem
        {
            get;
            set;
        }

        protected override void OnAdded(ContentItem item, int index)
        {
            if (item.IsMain)
            {
                TkDebug.Assert(MainItem == null, "不能有两个主表", this);
                MainItem = item;
            }
        }

        protected override void OnCleared()
        {
            MainItem = null;
        }

        protected override void OnRemoved(ContentItem item, int index)
        {
            if (item.IsMain)
                MainItem = null;
        }
    }
}