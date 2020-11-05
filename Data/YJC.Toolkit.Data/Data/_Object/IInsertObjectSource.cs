using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public interface IInsertObjectSource
    {
        object CreateNew(IInputData input);

        OutputData Insert(IInputData input, object instance);
    }
}
