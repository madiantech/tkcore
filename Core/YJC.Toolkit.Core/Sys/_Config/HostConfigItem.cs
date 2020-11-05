namespace YJC.Toolkit.Sys
{
    internal class HostConfigItem
    {
        [SimpleAttribute]
        public string Key { get; private set; }

        [SimpleAttribute]
        public string Value { get; private set; }
    }
}
