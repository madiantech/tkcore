using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class ListDataRightEventArgs : EventArgs
    {
        public ListDataRightEventArgs(TkDbContext context, IUserInfo user, IFieldInfoIndexer fieldIndexer)
        {
            Context = context;
            User = user;
            FieldIndexer = fieldIndexer;
        }

        public TkDbContext Context { get; private set; }

        public IUserInfo User { get; private set; }

        public IFieldInfoIndexer FieldIndexer { get; private set; }
    }
}
