using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.Excel
{
    public class ImportError : IEnumerable<ImportWarningItem>
    {
        private readonly List<ImportWarningItem> fResultList;

        public ImportError()
        {
            fResultList = new List<ImportWarningItem>();
        }

        internal void Add(ImportWarningItem iResult)
        {
            fResultList.Add(iResult);
        }

        public int Count
        {
            get
            {
                return fResultList.Count;
            }
        }

        IEnumerator<ImportWarningItem> IEnumerable<ImportWarningItem>.GetEnumerator()
        {
            return fResultList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return fResultList.GetEnumerator();
        }
    }
}
