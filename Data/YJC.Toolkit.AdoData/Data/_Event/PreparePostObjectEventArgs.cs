using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class PreparePostObjectEventArgs : FilledEventArgs
    {
        public PreparePostObjectEventArgs(IInputData inputData)
            : base(inputData)
        {
        }
    }
}
