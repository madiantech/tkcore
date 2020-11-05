using System;
using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class CompositeSource : ISource, ISupportMetaData, IPrepareSource, ISupportFunction
    {
        private class SourceInfo : IOrder
        {
            /// <summary>
            /// Initializes a new instance of the PageMakerInfo class.
            /// </summary>
            public SourceInfo(Func<IInputData, bool> function, IConfigCreator<ISource> sourceCreator, int order)
            {
                Function = function;
                SourceCreator = sourceCreator;
                Order = order;
            }

            public Func<IInputData, bool> Function { get; private set; }

            public IConfigCreator<ISource> SourceCreator { get; private set; }

            public int Order { get; private set; }
        }

        private readonly List<SourceInfo> fList;

        private ISource fCurrentSource;

        public CompositeSource()
        {
            fList = new List<SourceInfo>();
        }

        #region IPrepareSource 成员

        public void Prepare(IInputData input)
        {
            fCurrentSource = null;
            foreach (SourceInfo item in fList)
            {
                if (item.Function(input))
                {
                    ISource source = item.SourceCreator.CreateObject(input);
                    fCurrentSource = source;

                    IPrepareSource prepare = source as IPrepareSource;
                    if (prepare != null)
                        prepare.Prepare(input);

                    return;
                }
            }
            TkDebug.ThrowToolkitException("当前的状态下，没有一个注册的Source满足其条件，请检查条件", this);
        }

        #endregion IPrepareSource 成员

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return MetaDataUtil.CanUseMetaData(fCurrentSource, style);
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            MetaDataUtil.SetMetaData(fCurrentSource, style, metaData);
        }

        #endregion ISupportMetaData 成员

        #region ISupportFunction 成员

        public FunctionRightType FunctionType
        {
            get
            {
                if (fCurrentSource is ISupportFunction function)
                    return function.FunctionType;

                return FunctionRightType.None;
            }
        }

        public object FunctionKey
        {
            get
            {
                if (fCurrentSource is ISupportFunction function)
                    return function.FunctionKey;

                return null;
            }
        }

        #endregion ISupportFunction 成员

        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            return fCurrentSource.DoAction(input);
        }

        #endregion ISource 成员

        public ISource CurrentSource
        {
            get
            {
                return fCurrentSource;
            }
        }

        public void Add(Func<IInputData, bool> function, IConfigCreator<ISource> sourceCreator)
        {
            TkDebug.AssertArgumentNull(function, "function", this);
            TkDebug.AssertArgumentNull(sourceCreator, "source", this);

            fList.Add(new SourceInfo(function, sourceCreator, 0));
        }
    }
}