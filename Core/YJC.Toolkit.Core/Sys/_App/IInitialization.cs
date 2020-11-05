namespace YJC.Toolkit.Sys
{
    public interface IInitialization
    {
        void AppStarting(object application, BaseAppSetting appsetting,
            BaseGlobalVariable globalVariable);

        void AppStarted(object application, BaseAppSetting appsetting,
            BaseGlobalVariable globalVariable);

        void AppEnd(object application);
    }
}
