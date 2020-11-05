using YJC.Toolkit.Data;

namespace YJC.Toolkit.Web
{
    public interface IModuleCreator
    {
        IModule Create(string source);
    }
}
