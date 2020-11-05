using System;
using System.Collections;
using System.Collections.Generic;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class TableResolverCollection : IEnumerable<TableResolver>
    {
        private readonly RegNameList<TableResolver> fList;

        public TableResolverCollection()
        {
            fList = new RegNameList<TableResolver>();
        }

        #region IEnumerable<TableResolver> 成员

        public IEnumerator<TableResolver> GetEnumerator()
        {
            return fList.GetEnumerator();
        }

        #endregion IEnumerable<TableResolver> 成员

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable 成员

        public bool Add(TableResolver resolver)
        {
            if (fList.Contains(resolver))
            {
                var srcResolver = fList[resolver.TableName];
                srcResolver.MergeCommand(resolver);
                return false;
            }

            fList.Add(resolver);
            return true;
        }

        public TableResolver Add(string tableName, Func<TableResolver> creator)
        {
            TableResolver resolver = fList[tableName];
            if (resolver != null)
                return resolver;

            TkDebug.AssertArgumentNull(creator, "creator", this);
            resolver = creator();
            fList.Add(resolver);
            return resolver;
        }
    }
}