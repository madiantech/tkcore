using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class SingleDbInsertSource : BaseSingleDbMetaDataSource
    {
        protected SingleDbInsertSource()
        {
        }

        public SingleDbInsertSource(IEditDbConfig config)
            : base(config)
        {
        }

        protected override bool TestPageStyleForMetaData(IPageStyle style)
        {
            return MetaDataUtil.Equals(style, (PageStyleClass)PageStyle.Insert);
        }

        protected void DoInsertAction(IInputData input)
        {
            DataTable table = MainResolver.CreateVirtualTable();
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            MainResolver.SetDefaultValue(input.QueryString);
            MainResolver.SetDefaultValue(row);
            MetaDataTableResolver metaResolver = MainResolver as MetaDataTableResolver;
            if (metaResolver != null)
            {
                metaResolver.FillCodeTable(input.Style);
                metaResolver.Decode(input.Style);
            }
        }

        public override OutputData DoAction(IInputData input)
        {
            PageStyle style = input.Style.Style;
            if (style == PageStyle.Insert)
            {
                DoInsertAction(input);
                input.CallerInfo.AddInfo(DataSet);
            }
            else
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "当前支持页面类型为Insert，当前类型是{0}", input.Style), this);

            return OutputData.Create(DataSet);
        }
    }
}
