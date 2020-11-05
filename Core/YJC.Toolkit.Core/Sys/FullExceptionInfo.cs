using System;

namespace YJC.Toolkit.Sys
{
    [Serializable]
    internal class FullExceptionInfo : ExceptionInfo
    {
        protected override void SetInfo(Exception exception)
        {
            base.SetInfo(exception);

            if (exception.TargetSite != null)
                TargetSite = exception.TargetSite.ToString();
            ErrorSource = exception.Source;
        }
    }
}
