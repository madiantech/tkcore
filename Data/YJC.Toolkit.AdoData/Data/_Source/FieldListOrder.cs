using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class FieldListOrder
    {
        public FieldListOrder()
        {
            FieldList = new RegNameList<FieldOrder>();
        }

        [ObjectElement(UseConstructor = true, IsMultiple = true)]
        public RegNameList<FieldOrder> FieldList { get; private set; }

        public bool IsEmpty
        {
            get
            {
                return FieldList.Count == 0;
            }
        }

        public bool Contains(string nickName)
        {
            TkDebug.AssertArgumentNull(nickName, "nickName", this);

            return FieldList.ConstainsKey(nickName);
        }

        public string ToJson()
        {
            return this.WriteJson();
        }

        public string ToSqlOrder(IFieldInfoIndexer indexer)
        {
            TkDebug.AssertArgumentNull(indexer, "indexer", this);

            var orderList = from item in FieldList
                            let order = item.ToSqlOrder(indexer)
                            where order != null
                            select order;
            string orderBy = string.Join(",", orderList);
            if (string.IsNullOrEmpty(orderBy))
                return string.Empty;
            return "ORDER BY " + orderBy;
        }

        public static FieldListOrder FromSqlString(TableSelector selector, string orderBy)
        {
            FieldListOrder result = new FieldListOrder();
            if (string.IsNullOrEmpty(orderBy))
                return result;

            orderBy = orderBy.Trim();
            if (orderBy.StartsWith("order by", StringComparison.OrdinalIgnoreCase))
                orderBy = orderBy.Substring(8).Trim();
            string[] items = orderBy.Split(',');
            var fieldList = selector.FieldList;
            foreach (var item in items)
            {
                var fieldInfo = fieldList.FirstOrDefault(field =>
                    item.IndexOf(field.FieldName, StringComparison.Ordinal) != -1);
                if (fieldInfo != null)
                {
                    DbOrder order;
                    if (item.IndexOf("DESC", StringComparison.OrdinalIgnoreCase) != -1)
                        order = DbOrder.Desc;
                    else
                        order = DbOrder.Asc;
                    result.FieldList.Add(new FieldOrder(fieldInfo.NickName, order));
                }
            }
            return result;
        }

        public static FieldListOrder FromJson(string json)
        {
            TkDebug.AssertArgumentNullOrEmpty(json, "json", null);

            FieldListOrder result = new FieldListOrder();
            result.ReadJson(json);
            return result;
        }
    }
}