using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class FillingUpdateEventArgs : FilledUpdateEventArgs
    {
        public FillingUpdateEventArgs(IInputData inputData)
            : base(inputData)
        {
            Handled = new HandledCollection();
        }

        public HandledCollection Handled { get; private set; }
    }
}
