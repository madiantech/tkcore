using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class SwitchFieldSource : BaseDbSource
    {
        public SwitchFieldSource(TableResolver resolver, SwitchConfig @switch)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNull(@switch, "switch", null);

            Resolver = resolver;
            Switch = @switch;
        }

        internal SwitchFieldSource(SwitchFieldSourceConfig config)
        {
            if (!string.IsNullOrEmpty(config.Context))
                Context = DbContextUtil.CreateDbContext(config.Context);

            Resolver = config.Resolver.CreateObject(this);
            Switch = config.Switch;
        }

        public TableResolver Resolver { get; private set; }

        public SwitchConfig Switch { get; private set; }

        public override OutputData DoAction(IInputData input)
        {
            try
            {
                DataRow row = Resolver.Query(input.QueryString);
                Switch.Switch(Resolver, row);

                return OutputData.CreateToolkitObject(Resolver.CreateKeyData());
            }
            catch (WebPostException ex)
            {
                return OutputData.CreateToolkitObject(ex.CreateErrorResult());
            }
        }
    }
}
