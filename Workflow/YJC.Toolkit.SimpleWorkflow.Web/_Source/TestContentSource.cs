using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class TestContentSource : BaseDbSource
    {
        private readonly TableResolver fUserResolver;
        private readonly TableResolver fPartResolver;

        public TestContentSource()
        {
            fUserResolver = PlugInFactoryManager.CreateInstance<TableResolver>(
                ResolverPlugInFactory.REG_NAME, "User", this);
            fPartResolver = PlugInFactoryManager.CreateInstance<TableResolver>(
                ResolverPlugInFactory.REG_NAME, "Part", this);
        }

        public override OutputData DoAction(IInputData input)
        {
            fUserResolver.SelectWithKeys(1);
            fPartResolver.Select();

            return OutputData.Create(DataSet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fUserResolver.Dispose();
                fPartResolver.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}