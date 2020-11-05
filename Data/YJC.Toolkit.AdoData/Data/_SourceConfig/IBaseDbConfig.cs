using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public interface IBaseDbConfig
    {
        string Context { get; }

        bool SupportData { get; }

        IConfigCreator<IDataRight> DataRight { get; }

        FunctionRightConfig FunctionRight { get; }
    }
}