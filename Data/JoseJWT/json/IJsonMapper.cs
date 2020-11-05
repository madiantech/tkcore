using System.Collections.Generic;

namespace YJC.Toolkit.JWT
{
    public interface IJsonMapper
    {
        string Serialize(object obj);

        Dictionary<string, string> Parse(string json);
    }
}