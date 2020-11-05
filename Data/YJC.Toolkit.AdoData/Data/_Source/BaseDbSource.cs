using System.ComponentModel;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseDbSource : EmptyDbDataSource, ISource
    {
        protected BaseDbSource()
        {
            EventHandlers = new EventHandlerList();
        }

        #region ISource 成员

        public abstract OutputData DoAction(IInputData input);

        #endregion

        protected void SetConfig(IBaseDbConfig config)
        {
            TkDebug.AssertArgumentNull(config, "config", this);

            if (!string.IsNullOrEmpty(config.Context))
                Context = DbContextUtil.CreateDbContext(config.Context);
        }

        protected EventHandlerList EventHandlers { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                EventHandlers.Dispose();

            base.Dispose(disposing);
        }
    }
}
