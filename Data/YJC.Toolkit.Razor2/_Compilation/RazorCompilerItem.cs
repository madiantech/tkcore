using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class RazorCompilerItem : ICacheDependencyCreator
    {
        private readonly ICacheDependency fDependency;

        public RazorCompilerItem(Task<CompiledTemplateDescriptor> task, IChangeToken token)
        {
            Task = task;
            fDependency = new ChangeTokenDependency(token);
        }

        public Task<CompiledTemplateDescriptor> Task { get; }

        public ICacheDependency CreateCacheDependency()
        {
            return fDependency;
        }
    }
}