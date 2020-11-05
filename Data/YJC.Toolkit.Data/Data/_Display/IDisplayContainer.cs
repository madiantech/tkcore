using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal interface IDisplayContainer
    {
        void SetInternalDisplay(IConfigCreator<IDisplay> display);
    }
}
