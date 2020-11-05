namespace YJC.Toolkit.Sys
{
    internal class UploadConfigItem
    {
        //[SimpleAttribute]
        //public string Path { get; private set; }

        [SimpleAttribute]
        public string TempPath { get; private set; }

        [SimpleAttribute]
        public string TempVirtualPath { get; private set; }

        //[SimpleAttribute]
        //public string VirtualPath { get; private set; }
    }
}