namespace YJC.Toolkit.Sys.Json
{
    internal enum JsonWriteState
    {
        Error,
        Closed,
        Object,
        Array,
        Constructor,
        Property,
        Start
    }
}
