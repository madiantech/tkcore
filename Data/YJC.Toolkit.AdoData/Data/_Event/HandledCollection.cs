using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class HandledCollection
    {
        private readonly Dictionary<TableResolver, bool> fResolvers;

        /// <summary>
        /// Initializes a new instance of the HandledCollection class.
        /// </summary>
        public HandledCollection()
        {
            fResolvers = new Dictionary<TableResolver, bool>();
        }

        public bool IsHandled(TableResolver resolver)
        {
            try
            {
                bool value = fResolvers[resolver];
                return value;
            }
            catch
            {
                return false;
            }
        }

        public void SetHandled(TableResolver resolver, bool value)
        {
            TkDebug.Assert(fResolvers.ContainsKey(resolver), string.Format(
                ObjectUtil.SysCulture, "在Resolver集合中，没有找到({0})的Resolver。"
                + "请确认是否使用AddResolvers增加对应的Resolver", resolver), resolver);
            fResolvers[resolver] = value;
        }

        public void AddResolvers(params TableResolver[] resolvers)
        {
            foreach (TableResolver resolver in resolvers)
                fResolvers.Add(resolver, false);
        }

        //internal void AddResolvers(IEnumerable<WebBaseTableResolver> resolvers)
        //{
        //    foreach (TableResolver resolver in resolvers)
        //        fResolvers.Add(resolver, false);
        //}
    }
}
