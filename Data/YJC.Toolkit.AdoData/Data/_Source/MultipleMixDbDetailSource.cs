using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class MultipleMixDbDetailSource : BaseMultipleDbDetailSource
    {
        protected MultipleMixDbDetailSource()
        {
            OneToOneTables = new RegNameList<OneToOneChildTableInfo>();
        }

        internal MultipleMixDbDetailSource(IDetailDbConfig config)
            : base(config)
        {
            OneToOneTables = new RegNameList<OneToOneChildTableInfo>();
            IMultipleMixDbSourceConfig mix = config as IMultipleMixDbSourceConfig;
            if (mix != null)
            {
                var tableInfos = mix.OneToOneTables;
                if (tableInfos != null)
                    foreach (var item in tableInfos)
                    {
                        var info = new OneToOneChildTableInfo(this, item);
                        OneToOneTables.Add(info);
                    }
            }
            if (config.DetailOperators != null)
                Operators = config.DetailOperators.CreateObject();
        }

        public RegNameList<OneToOneChildTableInfo> OneToOneTables { get; private set; }

        public IOperatorsConfig Operators { get; set; }

        public bool FillDetailData { get; set; }

        private void MakeOperateRight(IInputData input)
        {
            IEnumerable<Operator> listOpertors = null;
            var operateRight = Operators.Right.CreateObject();
            if (operateRight == null)
            {
                var allOpertors = Operators.Operators;
                if (allOpertors != null)
                    listOpertors = from item in allOpertors
                                   select new Operator(item, this, input, MainResolver.GetKeyFieldArray());
            }
            else
            {
                IEnumerable<string> rights = operateRight.GetOperator(
                    new OperateRightEventArgs(input.Style, input.SourceInfo.Source, MainResolver.HostTable.Rows[0]));
                var allOpertors = Operators.Operators;
                if (rights != null && allOpertors != null)
                    listOpertors = from item in allOpertors
                                   join right in rights on item.Id equals right
                                   select new Operator(item, this, input, MainResolver.GetKeyFieldArray());
            }
            if (listOpertors != null)
            {
                DataTable table = listOpertors.CreateTable("DetailOperator");
                if (table != null)
                    DataSet.Tables.Add(table);
            }
        }

        protected override bool TestPageStyleForMetaData(IPageStyle style)
        {
            return MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.Detail) ||
                MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.Update);
        }

        public override OutputData DoAction(IInputData input)
        {
            var style = input.Style.Style;
            if (style == PageStyle.Detail || style == PageStyle.Update)
            {
                DoDetailAction(input);
                input.CallerInfo.AddInfo(DataSet);
            }
            else
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "当前支持页面类型为Detail，当前类型是{0}", input.Style), this);

            return OutputData.Create(DataSet);
        }

        protected void DoDetailAction(IInputData input)
        {
            DefaultUpdateAction(input);
        }

        protected override void DefaultUpdateAction(IInputData input)
        {
            base.DefaultUpdateAction(input);

            if (Operators != null && input.Style.Style == PageStyle.Detail)
            {
                if (MainResolver.HostTable.Rows.Count > 0)
                    MakeOperateRight(input);
            }

            MetaDataTableResolver metaResolver = MainResolver as MetaDataTableResolver;
            Decode(input, metaResolver);
            if (OneToOneTables != null)
            {
                foreach (var item in OneToOneTables)
                {
                    metaResolver = item.Resolver as MetaDataTableResolver;
                    Decode(input, metaResolver);
                }
            }
            if (FillDetailData)
                foreach (var resolver in ChildResolvers)
                {
                    metaResolver = resolver as MetaDataTableResolver;
                    Decode(input, metaResolver);
                }
        }

        protected override void FillUpdateTables(IInputData input)
        {
            base.FillUpdateTables(input);
            foreach (var childInfo in OneToOneTables)
            {
                var resolver = childInfo.Resolver;
                if (!FillingUpdateArgs.Handled.IsHandled(resolver))
                    base.FillUpdateTables(resolver, input, childInfo);
                DataTable table = resolver.HostTable;
                if (table.Rows.Count == 0)
                {
                    switch (childInfo.NoRecordHandler)
                    {
                        case NoRecordHandler.None:
                            break;

                        case NoRecordHandler.Error:
                            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                                "一对一子表[{0}]的数据无法对应，请确认", table.TableName), this);
                            break;

                        case NoRecordHandler.EmptyRecord:
                            DataRow row = table.NewRow();
                            table.Rows.Add(row);
                            if (input.Style.Style == PageStyle.Update)
                                resolver.SetDefaultValue(row);
                            break;
                    }
                }
            }
        }

        protected override void FillUpdateTables(TableResolver resolver,
            IInputData input, ChildTableInfo childInfo)
        {
            if (FillDetailData || childInfo == null)
                base.FillUpdateTables(resolver, input, childInfo);
        }

        private static void Decode(IInputData input, MetaDataTableResolver metaResolver)
        {
            if (metaResolver != null)
            {
                metaResolver.Decode(input.Style);

                if (input.Style.Style == PageStyle.Update)
                    metaResolver.FillCodeTable(input.Style);
            }
        }
    }
}