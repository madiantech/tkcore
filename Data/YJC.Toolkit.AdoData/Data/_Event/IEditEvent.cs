using System;

namespace YJC.Toolkit.Data
{
    public interface IEditEvent : IDetailEvent
    {
        event EventHandler<FilledInsertEventArgs> FilledInsertTables;

        event EventHandler<PreparePostObjectEventArgs> PreparedPostObject;
    }
}
