
namespace YJC.Toolkit.Sys
{
    internal class AppUrlConfigItem
    {
        [SimpleAttribute]
        public string StartupPath { get; private set; }

        [SimpleAttribute]
        public string LogOnPath { get; private set; }

        [SimpleAttribute]
        public string HomePath { get; private set; }

        [SimpleAttribute]
        public string MainPath { get; private set; }

        [SimpleAttribute]
        public string ErrorPage { get; private set; }
    }
}
