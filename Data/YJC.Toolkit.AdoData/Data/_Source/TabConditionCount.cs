using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class TabConditionCount
    {
        public class TabItemCount
        {
            public TabItemCount(string id, int count)
            {
                Id = id;
                Count = count;
            }

            [SimpleAttribute]
            public string Id { get; private set; }

            [SimpleAttribute]
            public int Count { get; private set; }
        }

        public TabConditionCount()
        {
            TabList = new List<TabItemCount>();
        }

        [ObjectElement(IsMultiple = true)]
        public List<TabItemCount> TabList { get; private set; }

        public void Add(string id, int count)
        {
            TkDebug.AssertArgumentNullOrEmpty(id, "id", this);

            TabList.Add(new TabItemCount(id, count));
        }
    }
}