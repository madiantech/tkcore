using System.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseSingleDbSource : BaseDbSource, ISupportFunction
    {
        private TableResolver fMainResolver;

        protected BaseSingleDbSource()
        {
        }

        protected BaseSingleDbSource(IBaseDbConfig config)
        {
            TkDebug.AssertArgumentNull(config, "config", null);

            SetConfig(config);
            MainResolver = DbConfigUtil.CreateSingleTableResolver(config, this);
            if (config.DataRight != null)
            {
                SupportData = config.SupportData;
                DataRight = config.DataRight.CreateObject(MainResolver);
            }
            if (config.FunctionRight != null)
            {
                FunctionKey = config.FunctionRight.FunctionKey;
                FunctionType = config.FunctionRight.FunctionType;
            }
        }

        public bool SupportData { get; protected set; }

        public TableResolver MainResolver
        {
            get
            {
                return fMainResolver;
            }
            protected set
            {
                fMainResolver = value;
                if (value != null)
                    OnSetMainResolver(value);
            }
        }

        public IDataRight DataRight { get; protected set; }

        public FunctionRightType FunctionType { get; set; }

        public object FunctionKey { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                fMainResolver.DisposeObject();

            base.Dispose(disposing);
        }

        protected virtual void OnSetMainResolver(TableResolver resolver)
        {
        }

        protected virtual void CheckDataRight(IInputData input, DataRow row)
        {
            TkDebug.ThrowIfNoGlobalVariable();
            var args = new DataRightEventArgs(Context, BaseGlobalVariable.Current.UserInfo,
                MainResolver, input.Style, row);
            DataRight.Check(args);
        }
    }
}