using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class FilledInsertEventArgs : FilledEventArgs
    {
        public FilledInsertEventArgs(IInputData inputData)
            : base(inputData)
        {
        }
    }
}
