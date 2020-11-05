using System;

namespace YJC.Toolkit.Data
{
    public interface IDetailEvent
    {
        event EventHandler<FillingUpdateEventArgs> FillingUpdateTables;

        event EventHandler<FilledUpdateEventArgs> FilledUpdateTables;
    }
}
