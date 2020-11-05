using System;

namespace YJC.Toolkit.Sys
{
    internal class RegItemEventArgs : EventArgs
    {
        public RegItemEventArgs(string regName)
        {
            RegName = regName;
        }

        public string RegName { get; private set; }

        public BaseRegItem RegItem { get; set; }
    }
}
