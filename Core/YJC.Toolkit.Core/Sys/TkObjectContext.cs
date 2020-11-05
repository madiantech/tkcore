using System.Collections;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public sealed class TkObjectContext : IEnumerable<object>
    {
        private readonly Stack<object> fObjectStack;

        public TkObjectContext()
        {
            fObjectStack = new Stack<object>();
        }

        #region IEnumerable<object> 成员

        public IEnumerator<object> GetEnumerator()
        {
            return fObjectStack.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Push(object obj)
        {
            if (obj == null)
                return;

            fObjectStack.Push(obj);
        }

        public object Pop()
        {
            if (fObjectStack.Count == 0)
                return null;

            return fObjectStack.Pop();
        }

        public void Clear()
        {
            fObjectStack.Clear();
        }

        public TkObjectContext Clone()
        {
            TkObjectContext result = new TkObjectContext();
            if (fObjectStack.Count == 0)
                return result;

            foreach (object item in this)
                result.fObjectStack.Push(item);
            return result;
        }
    }
}
