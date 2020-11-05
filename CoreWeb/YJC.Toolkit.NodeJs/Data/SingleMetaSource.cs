using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class SingleMetaSource : BaseDbSource, ISupportMetaData
    {
        private static IPageStyle Insert = PageStyleClass.FromString("InsertVue");
        private static IPageStyle Update = PageStyleClass.FromString("UpdateVue");

        public SingleMetaSource()
        {
        }

        internal SingleMetaSource(SingleMetaSourceConfig config)
        {
            UseMetaData = config.UseMetaData;
            MainResolver = config.Resolver.CreateObject(this);
        }

        public bool UseMetaData { get; set; }

        public TableResolver MainResolver { get; set; }

        public bool CanUseMetaData(IPageStyle style)
        {
            return UseMetaData;
        }

        public override OutputData DoAction(IInputData input)
        {
            var style = input.Style;
            if (MetaDataUtil.Equals(style, Insert) || MetaDataUtil.Equals(style, Update))
            {
                DoMetaAction(input);
                input.CallerInfo.AddInfo(DataSet);
            }
            else
                TkDebug.ThrowToolkitException(
                    $"当前支持页面类型为{Insert}或{Update}，当前类型是{input.Style}", this);

            return OutputData.Create(DataSet);
        }

        private void DoMetaAction(IInputData input)
        {
            if (MainResolver is MetaDataTableResolver metaResover)
            {
                metaResover.FillCachedCodeTable(input.Style);
            }
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            ITableSchemeEx scheme = metaData.GetTableScheme(MainResolver.TableName);
            if (scheme != null)
            {
                MainResolver.ReadMetaData(scheme);
                OnReadMetaData(MainResolver, style, scheme);
            }
        }

        protected virtual void OnReadMetaData(TableResolver resolver, IPageStyle style,
            ITableSchemeEx scheme)
        {
        }
    }
}