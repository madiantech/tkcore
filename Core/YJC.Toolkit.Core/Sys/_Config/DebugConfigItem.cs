namespace YJC.Toolkit.Sys
{
    internal class DebugConfigItem
    {
        [SimpleAttribute]
        public bool Debug { get; protected set; }

        [SimpleAttribute]
        public bool ShowException { get; protected set; }
    }
}