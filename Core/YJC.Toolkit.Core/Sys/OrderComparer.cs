using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public sealed class OrderComparer : IComparer<IOrder>
    {
        public static readonly IComparer<IOrder> Comparer = new OrderComparer();

        private OrderComparer()
        {
        }

        #region IComparer<IOrder> 成员

        public int Compare(IOrder x, IOrder y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            if (x.Order < y.Order)
                return -1;
            else if (x.Order == y.Order)
                return 0;
            else
                return 1;
        }

        #endregion
    }
}
