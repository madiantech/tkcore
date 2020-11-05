using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal interface IObjectFormat
    {
        ConfigType GZip { get; }

        ConfigType Encrypt { get; }
    }
}
