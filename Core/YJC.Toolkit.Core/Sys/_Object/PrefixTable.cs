using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YJC.Toolkit.Sys
{
    internal sealed class PrefixTable : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly Dictionary<string, string> fTable;
        private int fIndex;
        private bool fUsed;

        /// <summary>
        /// Initializes a new instance of the NamespaceTable class.
        /// </summary>
        public PrefixTable()
        {
            fTable = new Dictionary<string, string>();
            fTable.Add(ToolkitConst.NAMESPACE_URL, ToolkitConst.TK_NS);
            fIndex = 1;
        }

        public string GetPrefix(string namespaceName)
        {
            fUsed = true;
            if (fTable.ContainsKey(namespaceName))
                return fTable[namespaceName];
            else
            {
                string prefix = "a" + (fIndex++);
                fTable.Add(namespaceName, prefix);
                return prefix;
            }
        }

        #region IEnumerable<KeyValuePair<string,string>> 成员

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            if (!fUsed)
                return Enumerable.Empty<KeyValuePair<string, string>>().GetEnumerator();
            return fTable.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
