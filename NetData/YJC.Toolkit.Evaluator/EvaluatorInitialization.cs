namespace YJC.Toolkit.Sys
{
    internal class EvaluatorInitialization : IInitialization
    {
        #region IInitialization 成员

        public void AppStarting(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
            EvaluatorExtension.SetExtension(new EvaluatorExtensionImpl());
        }

        public void AppStarted(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
        }

        public void AppEnd(object application)
        {
        }

        #endregion IInitialization 成员
    }
}