using System.Collections;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [TkTypeConverter(typeof(ObjectOperatorCollectionTypeConverter))]
    public sealed class ObjectOperatorCollection : IEnumerable<string>
    {
        private readonly HashSet<string> fOperators;

        private ObjectOperatorCollection()
        {
            fOperators = new HashSet<string>();
        }

        public ObjectOperatorCollection(string operatorId)
            : this()
        {
            Add(operatorId);
        }

        public ObjectOperatorCollection(params string[] operatorIds)
            : this((IEnumerable<string>)operatorIds)
        {
        }

        public ObjectOperatorCollection(IEnumerable<string> operatorIds)
            : this()
        {
            if (operatorIds != null)
                foreach (var item in operatorIds)
                    Add(item);
        }

        #region IEnumerable<string> 成员

        public IEnumerator<string> GetEnumerator()
        {
            return fOperators.GetEnumerator();
        }

        #endregion IEnumerable<string> 成员

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable 成员

        public int Count
        {
            get
            {
                return fOperators.Count;
            }
        }

        public void Add(string operatorId)
        {
            if (!string.IsNullOrEmpty(operatorId))
                fOperators.Add(operatorId);
        }

        public bool Contains(string operatorId)
        {
            if (string.IsNullOrEmpty(operatorId))
                return false;

            return fOperators.Contains(operatorId);
        }

        public override string ToString()
        {
            if (fOperators.Count == 0)
                return string.Empty;
            return string.Format(ObjectUtil.SysCulture, "|{0}|", string.Join("|", fOperators));
        }
    }
}