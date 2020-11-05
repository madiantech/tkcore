using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class FieldOrder : IRegName
    {
        protected FieldOrder()
        {
        }

        public FieldOrder(string nickName, DbOrder order)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", null);

            NickName = nickName;
            Order = order;
        }

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return NickName;
            }
        }

        #endregion IRegName 成员

        [SimpleAttribute]
        public string NickName { get; private set; }

        [SimpleAttribute]
        public DbOrder Order { get; private set; }

        public string ToSqlOrder(IFieldInfoIndexer indexer)
        {
            TkDebug.AssertArgumentNull(indexer, "indexer", this);

            var fieldInfo = indexer[NickName];
            if (fieldInfo == null)
                return null;
            string fieldName = fieldInfo.FieldName;
            bool isSimple = true;
            if (fieldInfo is ITk5FieldInfo fieldInfoEx)
            {
                if (!string.IsNullOrEmpty(fieldInfoEx.ListDetail?.SortField))
                {
                    fieldName = fieldInfoEx.ListDetail.SortField;
                    isSimple = fieldName.IndexOfAny(new char[] { '(', ')' }) == -1;
                }
            }
            if (isSimple)
                return Order == DbOrder.Asc ? fieldName : fieldName + " DESC";
            else
            {
                Order = DbOrder.Asc;
                return fieldName;
            }
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", NickName, Order);
        }
    }
}