using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DbDetailStatListSource : DbStatListSource
    {
        private readonly TableResolver fMasterResolver;
        private readonly ChildTableInfo fChildInfo;

        public DbDetailStatListSource(IBaseDbConfig config, IConfigCreator<TableResolver> mainResolver,
            ChildTableInfoConfig childInfoConfig)
            : base(childInfoConfig.Stat)
        {
            TkDebug.AssertArgumentNull(config, "config", null);
            TkDebug.AssertArgumentNull(mainResolver, "mainResolver", null);
            TkDebug.AssertArgumentNull(childInfoConfig, "childInfoConfig", null);

            SetConfig(config);
            fChildInfo = new ChildTableInfo(this, childInfoConfig);
            // 子列表不该占有主配置的数据权限
            //if (config.DataRight != null)
            //{
            //    SupportData = config.SupportData;
            //    DataRight = config.DataRight.CreateObject(fChildInfo.Resolver);
            //}
            // 子列表不该占有功能权限
            FunctionType = FunctionRightType.None;

            OrderBy = fChildInfo.Relation.OrderBy;
            FilterSql = fChildInfo.Relation.FilterSql;
            MainResolver = fChildInfo.Resolver;
            fMasterResolver = mainResolver.CreateObject(this);
            if (childInfoConfig.Operators != null)
                Operators = childInfoConfig.Operators.CreateObject();
        }

        internal DbDetailStatListSource(DbDetailStatListSourceConfig config,
            IConfigCreator<TableResolver> mainResolver, ChildTableInfoConfig childInfoConfig)
            : this((IBaseDbConfig)config, mainResolver, childInfoConfig)
        {
            PageSize = config.PageSize;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fMasterResolver.Dispose();
                fChildInfo.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override IParamBuilder CreateCustomCondition(IInputData input)
        {
            return fChildInfo.CreateDetailParamBuilder(fMasterResolver, input);
        }

        protected override void CreateListOperators(IInputData input, ref IOperateRight operateRight)
        {
            base.CreateListOperators(input, ref operateRight);
        }

        public override OutputData DoAction(IInputData input)
        {
            if (MetaDataUtil.StartsWith(input.Style, "DetailList"))
            {
                InputDataProxy proxy = new InputDataProxy(input, (PageStyleClass)PageStyle.List);
                DoListAction(proxy);
                input.CallerInfo.AddInfo(DataSet);

                return OutputData.Create(DataSet);
            }
            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                "当前支持页面类型为CDetailList，当前类型是{0}", input.Style), this);

            return null;
        }
    }
}