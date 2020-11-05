using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public interface IDetailObjectSource
    {
        object Query(IInputData input, string id);
    }
}
