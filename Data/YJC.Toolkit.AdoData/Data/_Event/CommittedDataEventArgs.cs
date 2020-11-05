using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class CommittedDataEventArgs : FilledEventArgs
    {
        public CommittedDataEventArgs(IInputData inputData)
            : base(inputData)
        {
        }
    }
}
