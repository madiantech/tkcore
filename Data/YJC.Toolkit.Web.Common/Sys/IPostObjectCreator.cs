using System.IO;

namespace YJC.Toolkit.Sys
{
    public interface IPostObjectCreator
    {
        object Read(IInputData input, Stream stream);
    }
}
