namespace YJC.Toolkit.Sys
{
    public interface IConfig
    {
        object GetConfig(string sectionName);

        void RegisterConfig(ConfigTypeFactory factory);
    }
}