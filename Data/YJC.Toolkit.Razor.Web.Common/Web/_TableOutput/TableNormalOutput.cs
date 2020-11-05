using YJC.Toolkit.Razor;

namespace YJC.Toolkit.Web
{
    public class TableNormalOutput : RazorTableOutput
    {
        public TableNormalOutput()
          : this(false)
        {
        }

        public TableNormalOutput(bool directShowDetail)
            : base("TableOutput/editnormaltable.cshtml",
                  directShowDetail ? "TableOutput/detailtablelist.cshtml" : "TableOutput/detailtable.cshtml",
                  "TableOutput/detailtablehead.cshtml")
        {
        }

        public int ColumnCount { get; set; }

        public bool IsFix { get; set; }

        public RazorOutputData OtherNewButton { get; set; }

        protected override object CreateCustomData(bool isEdit)
        {
            if (isEdit)
                return (ColumnCount: ColumnCount, IsFix: IsFix, OtherNewButton: OtherNewButton);
            return null;
        }
    }
}