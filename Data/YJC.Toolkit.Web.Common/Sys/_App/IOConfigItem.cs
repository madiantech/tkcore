namespace YJC.Toolkit.Sys
{
    internal sealed class IOConfigItem
    {
        [SimpleAttribute]
        public bool InputGZip { get; private set; }

        [SimpleAttribute]
        public bool InputEncrypt { get; private set; }

        [SimpleAttribute]
        public bool OutputGZip { get; private set; }

        [SimpleAttribute]
        public bool OutputEncrypt { get; private set; }
    }
}
