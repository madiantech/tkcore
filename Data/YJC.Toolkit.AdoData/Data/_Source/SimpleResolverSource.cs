using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class SimpleResolverSource : BaseDbSource
    {
        protected SimpleResolverSource()
        {
        }

        internal SimpleResolverSource(SimpleResolverSourceConfig config)
        {
            OrderBy = config.OrderBy;
            FilterSql = config.FilterSql;
            MainResolver = config.Resolver.CreateObject(this);
            UseCallerInfo = config.UserCallerInfo;
        }

        public string OrderBy { get; protected set; }

        public bool UseCallerInfo { get; set; }

        public MarcoConfigItem FilterSql { get; protected set; }

        public TableResolver MainResolver { get; protected set; }

        public override OutputData DoAction(IInputData input)
        {
            if (MainResolver != null)
            {
                IParamBuilder builder;
                if (FilterSql != null)
                    builder = ParamBuilder.CreateSql(Expression.Execute(FilterSql, this));
                else
                    builder = ParamBuilder.CreateSql("1=1");
                MainResolver.Select(builder, OrderBy);

                MetaDataTableResolver metaResolver = MainResolver as MetaDataTableResolver;
                if (metaResolver != null)
                    metaResolver.Decode(input.Style);
            }

            if (UseCallerInfo)
                input.CallerInfo.AddInfo(DataSet);

            return OutputData.Create(DataSet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                MainResolver.DisposeObject();
            }
            base.Dispose(disposing);
        }
    }
}
