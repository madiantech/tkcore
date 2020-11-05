using System;

namespace YJC.Toolkit.Data
{
    public sealed class FilledListEventArgs : EventArgs
    {
        internal FilledListEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the FilledListEventArgs class.
        /// </summary>
        public FilledListEventArgs(bool isPost, int pageNumber, int pageSize, int count,
            string order, TableSelector listView, Object postObject, IParamBuilder condition)
        {
            IsPost = isPost;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Count = count;
            Order = order;
            ListView = listView;
            PostObject = postObject;
            Condition = condition;
        }

        public bool IsPost { get; internal set; }

        public int PageNumber { get; internal set; }

        public int PageSize { get; set; }

        public int Count { get; internal set; }

        public string Order { get; internal set; }

        public TableSelector ListView { get; internal set; }

        public Object PostObject { get; internal set; }

        public IParamBuilder Condition { get; internal set; }
    }
}