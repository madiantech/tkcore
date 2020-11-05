using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class ChangeTokenDependency : ICacheDependency
    {
        public ChangeTokenDependency(IChangeToken changeToken)
        {
            ChangeToken = changeToken;
        }

        public bool HasChanged => ChangeToken != null ? ChangeToken.HasChanged : false;

        public IChangeToken ChangeToken { get; }
    }
}