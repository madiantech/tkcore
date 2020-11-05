namespace YJC.Toolkit.Sys.Json
{
    internal enum JsonTokenType
    {
        None,
        Object,
        Array,
        Constructor,
        Property,
        Comment,
        Integer,
        Float,
        String,
        Boolean,
        Null,
        Undefined,
        Date,
        Raw
    }
}