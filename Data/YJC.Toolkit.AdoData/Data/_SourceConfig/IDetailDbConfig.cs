using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public interface IDetailDbConfig : IEditDbConfig
    {
        IConfigCreator<IOperatorsConfig> DetailOperators { get; }
    }
}
