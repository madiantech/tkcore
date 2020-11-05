using System;

namespace YJC.Toolkit.Data
{
    public interface IListEvent
    {
        event EventHandler<FilledListEventArgs> FilledListTables;
    }
}
