using System.Collections;

namespace YJC.Toolkit.Weixin
{
    internal class WeixinStringComparer : IComparer
    {
        public static readonly IComparer Comparer = new WeixinStringComparer();

        private WeixinStringComparer()
        {
        }

        public int Compare(object oLeft, object oRight)
        {
            string sLeft = oLeft as string;
            string sRight = oRight as string;
            if (sLeft == null && sRight == null)
                return 0;
            else if (sLeft == null)
                return -1;
            else if (sRight == null)
                return 1;

            int iLeftLength = sLeft.Length;
            int iRightLength = sRight.Length;
            int index = 0;
            while (index < iLeftLength && index < iRightLength)
            {
                if (sLeft[index] < sRight[index])
                    return -1;
                else if (sLeft[index] > sRight[index])
                    return 1;
                else
                    index++;
            }

            return iLeftLength - iRightLength;
        }
    }
}