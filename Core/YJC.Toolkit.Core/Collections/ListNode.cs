using System.Diagnostics;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Collections
{
    [DebuggerDisplay("{Value}")]
    internal class ListNode<T>
    {
        /// <summary>
        /// Initializes a new instance of the ListNode class.
        /// </summary>
        public ListNode(LinkList<T> list, T value)
        {
            Value = value;
            List = list;
        }

        public T Value { get; set; }

        public LinkList<T> List { get; private set; }

        public ListNode<T> Prev { get; set; }

        public ListNode<T> Next { get; set; }
    }
}
