using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;

namespace YJC.Toolkit.Data
{
    public sealed class ObjectListModel
    {
        private Operator[] fListOperators;
        private Operator[] fRowOperators;

        public ObjectListModel()
        {
            CallerInfo = new ExpandoObject();
        }

        public IEnumerable<ObjectContainer> List { get; private set; }

        public CountInfo Count { get; private set; }

        public ListSortInfo Sort { get; private set; }

        public dynamic CallerInfo { get; private set; }

        public int ListOperatorCount
        {
            get
            {
                return fListOperators == null ? 0 : fListOperators.Length;
            }
        }

        public IEnumerable<Operator> ListOperators
        {
            get
            {
                return fListOperators;
            }
            internal set
            {
                if (fListOperators != value)
                {
                    if (value != null)
                        fListOperators = value.ToArray();
                    else
                        fListOperators = null;
                }
            }
        }

        public IEnumerable<Operator> RowOperators
        {
            get
            {
                return fRowOperators;
            }
            internal set
            {
                if (fRowOperators != value)
                {
                    if (value != null)
                        fRowOperators = value.ToArray();
                    else
                        fRowOperators = null;
                }
            }
        }

        internal void SetPageInfo(Tuple<ListSortInfo, CountInfo, object> info)
        {
            Sort = info.Item1;
            Count = info.Item2;
        }

        internal void SetList(IEnumerable list, IEnumerable<IFieldInfoEx> metaFields)
        {
            var objList = new List<ObjectContainer>(Count.PageSize);
            List = objList;
            if (list == null)
                return;

            foreach (var item in list)
            {
                ObjectContainer listItem = new ObjectContainer(item);
                listItem.Decode(metaFields);
                objList.Add(listItem);
            }
        }
    }
}