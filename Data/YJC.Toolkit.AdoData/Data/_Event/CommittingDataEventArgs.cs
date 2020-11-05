using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class CommittingDataEventArgs : FilledEventArgs
    {
        public CommittingDataEventArgs(IInputData inputData)
            : base(inputData)
        {
        }
    }
}
