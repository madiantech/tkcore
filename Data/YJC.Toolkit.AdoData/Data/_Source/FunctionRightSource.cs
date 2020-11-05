using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class FunctionRightSource : ISource, ISupportMetaData, ISupportFunction, IPrepareSource
    {
        private readonly ISource fSource;
        private readonly FunctionRightConfig fFunctionRight;

        public FunctionRightSource(ISource source, FunctionRightConfig config)
        {
            TkDebug.AssertArgumentNull(source, nameof(source), null);
            TkDebug.AssertArgumentNull(config, nameof(config), null);

            fSource = source;
            fFunctionRight = config;
        }

        public FunctionRightType FunctionType { get => fFunctionRight.FunctionType; }

        public object FunctionKey { get => fFunctionRight.FunctionKey; }

        public bool CanUseMetaData(IPageStyle style)
        {
            return MetaDataUtil.CanUseMetaData(fSource, style);
        }

        public OutputData DoAction(IInputData input)
        {
            return fSource.DoAction(input);
        }

        public void Prepare(IInputData input)
        {
            if (fSource is IPrepareSource prepareSource)
                prepareSource.Prepare(input);
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            MetaDataUtil.SetMetaData(fSource, style, metaData);
        }
    }
}