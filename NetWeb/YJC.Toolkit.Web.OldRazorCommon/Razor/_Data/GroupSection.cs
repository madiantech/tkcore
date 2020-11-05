using System;
using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class GroupSection : IComparable<GroupSection>
    {
        #region IComparable<GroupSection> 成员

        public int CompareTo(GroupSection other)
        {
            if (other == null)
                return 1;
            return this.Order - other.Order;
        }

        #endregion IComparable<GroupSection> 成员

        [SimpleAttribute(Required = true)]
        public int Order { get; private set; }

        [SimpleAttribute]
        public bool IsCollaspe { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public MultiLanguageText Caption { get; private set; }

        [SimpleAttribute]
        public int EndOrder { get; internal set; }

        public static void SetEndOrder(List<GroupSection> groups)
        {
            for (int i = 0; i < groups.Count - 1; ++i)
            {
                var group = groups[i];
                if (group.EndOrder == 0)
                    group.EndOrder = groups[i + 1].Order;
            }
            if (groups.Count > 0)
            {
                var group = groups[groups.Count - 1];
                if (group.EndOrder == 0)
                    group.EndOrder = int.MaxValue;
            }
        }
    }
}