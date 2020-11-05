using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class SimpleFieldLayout : IFieldLayout
    {
        public SimpleFieldLayout()
        {
        }

        public SimpleFieldLayout(IFieldLayout layout)
        {
            if (layout == null)
            {
                Layout = FieldLayout.PerUnit;
                UnitNum = 1;
            }
            else
            {
                Layout = layout.Layout;
                UnitNum = layout.UnitNum;
            }
        }

        [SimpleAttribute(DefaultValue = FieldLayout.PerUnit)]
        public FieldLayout Layout { get; set; }

        [SimpleAttribute(DefaultValue = 1)]
        public int UnitNum { get; set; }

        internal void Override(OverrideLayoutConfig config)
        {
            if (config.Layout.HasValue)
                Layout = config.Layout.Value;
            if (config.UnitNum.HasValue)
                UnitNum = config.UnitNum.Value;
        }

        public override string ToString()
        {
            if (Layout == FieldLayout.PerLine)
                return "[PerLine]";
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", Layout, UnitNum);
        }

        public static IFieldLayout CreateDefault()
        {
            return new SimpleFieldLayout
            {
                Layout = FieldLayout.PerUnit,
                UnitNum = 1
            };
        }
    }
}