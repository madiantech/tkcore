using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class MultipleDbDetailSource : BaseMultipleDbDetailSource
    {
        protected MultipleDbDetailSource()
        {
        }

        public MultipleDbDetailSource(IDetailDbConfig config)
            : base(config)
        {
            if (config.DetailOperators != null)
                Operators = config.DetailOperators.CreateObject();
        }

        protected override bool TestPageStyleForMetaData(IPageStyle style)
        {
            return MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.Detail) ||
                MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.Update);
        }

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

        private static void Decode(IInputData input, MetaDataTableResolver metaResolver)
        {
            if (metaResolver != null)
            {
                metaResolver.Decode(input.Style);

                if (input.Style.Style == PageStyle.Update)
                    metaResolver.FillCodeTable(input.Style);
            }
        }

        public override OutputData DoAction(IInputData input)
        {
            PageStyle style = input.Style.Style;
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

        protected override void FillUpdateTables(TableResolver resolver, IInputData input,
            ChildTableInfo childInfo)
        {
            if (FillDetailData || childInfo == null)
                base.FillUpdateTables(resolver, input, childInfo);
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

            if (FillDetailData)
            {
                foreach (TableResolver resolver in ChildResolvers)
                {
                    metaResolver = resolver as MetaDataTableResolver;
                    Decode(input, metaResolver);
                }
            }
        }
    }
}