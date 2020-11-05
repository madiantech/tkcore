using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public interface IUpdateObjectSource : IDetailObjectSource
    {
        OutputData Update(IInputData input, object instance);
    }
}
