using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public interface IDeleteObjectSource
    {
        OutputData Delete(IInputData input, string id);
    }
}
