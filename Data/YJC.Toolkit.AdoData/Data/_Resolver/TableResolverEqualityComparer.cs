using System.Collections.Generic;

namespace YJC.Toolkit.Data
{
    public sealed class TableResolverEqualityComparer : IEqualityComparer<TableResolver>
    {
        public static readonly IEqualityComparer<TableResolver> Comparer
            = new TableResolverEqualityComparer();

        private TableResolverEqualityComparer()
        {
        }

        #region IEqualityComparer<TableResolver> 成员

        public bool Equals(TableResolver x, TableResolver y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            return x.TableName == y.TableName;
        }

        public int GetHashCode(TableResolver obj)
        {
            if (obj != null)
                return obj.TableName.GetHashCode();
            return GetHashCode();
        }

        #endregion
    }
}
