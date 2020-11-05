namespace YJC.Toolkit.Sys
{
    public interface IConfigCreator<T>
    {
        T CreateObject(params object[] args);
    }
}
