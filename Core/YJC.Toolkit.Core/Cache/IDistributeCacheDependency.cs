using System;
using System.Collections.Generic;
using System.Text;

namespace YJC.Toolkit.Cache
{
    public interface IDistributeCacheDependency : ICacheDependency
    {
        object CreateStoredObject();
    }
}