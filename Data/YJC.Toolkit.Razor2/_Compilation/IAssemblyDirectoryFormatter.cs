using System.Reflection;

namespace YJC.Toolkit.Razor
{
    public interface IAssemblyDirectoryFormatter
    {
        string GetAssemblyDirectory(Assembly assembly);
    }
}