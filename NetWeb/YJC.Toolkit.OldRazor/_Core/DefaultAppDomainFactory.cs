using System;

namespace YJC.Toolkit.Razor
{
    internal class DefaultAppDomainFactory : IAppDomainFactory
    {
        public AppDomain CreateAppDomain()
        {
            var current = AppDomain.CurrentDomain;
            var domain = AppDomain.CreateDomain("RazorHost", current.Evidence, current.SetupInformation);

            return domain;
        }
    }
}
