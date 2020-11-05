using System.Collections.Generic;
using System.Linq;

namespace YJC.Toolkit.Right
{
    public class LinkOperateRight : IOperateRight
    {
        private readonly IEnumerable<IOperateRight> fRights;

        public LinkOperateRight(IEnumerable<IOperateRight> rights)
        {
            if (rights != null)
            {
                fRights = from item in rights
                          where item != null
                          select item;
            }
        }

        #region IOperateRight 成员

        public IEnumerable<string> GetOperator(OperateRightEventArgs e)
        {
            if (fRights == null)
                return null;

            var items = from right in fRights
                        let opers = right.GetOperator(e)
                        where opers != null
                        select opers;
            var first = items.FirstOrDefault();
            if (first == null)
                return null;

            var intersection = items.Skip(1)
                .Aggregate(new HashSet<string>(first),
                (h, a) => { h.IntersectWith(a); return h; });

            return intersection;
        }

        #endregion IOperateRight 成员
    }
}