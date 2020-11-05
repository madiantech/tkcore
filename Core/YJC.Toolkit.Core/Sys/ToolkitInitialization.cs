namespace YJC.Toolkit.Sys
{
    internal class ToolkitInitialization : IInitialization
    {
        #region IInitialization 成员

        public void AppStarting(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
        }

        public void AppStarted(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
            TkDebug.ThrowIfNoGlobalVariable();
            EvaluateAdditionPlugInFactory factory = BaseGlobalVariable.Current.FactoryManager.GetCodeFactory(
                EvaluateAdditionPlugInFactory.REG_NAME).Convert<EvaluateAdditionPlugInFactory>();
            factory.Initialize();
        }

        public void AppEnd(object application)
        {
        }

        #endregion IInitialization 成员
    }
}