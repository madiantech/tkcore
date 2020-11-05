using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class FilledEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the FilledEventArgs class.
        /// </summary>
        public FilledEventArgs(IInputData inputData)
        {
            InputData = inputData;
        }

        public IInputData InputData { get; private set; }
    }
}
