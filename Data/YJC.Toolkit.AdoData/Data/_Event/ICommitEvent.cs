using System;

namespace YJC.Toolkit.Data
{
    public interface ICommitEvent
    {
        event EventHandler<CommittingDataEventArgs> CommittingData;

        event EventHandler<CommittedDataEventArgs> CommittedData;

        event EventHandler<ApplyDataEventArgs> ApplyData;
    }
}
