using YJC.Toolkit.Razor;

namespace YJC.Toolkit.Web
{
    public class TableOutput : RazorTableOutput
    {
        public TableOutput()
            : this(false)
        {
        }

        public TableOutput(bool directShowDetail)
            : base("TableOutput/edittable.cshtml",
                  directShowDetail ? "TableOutput/detailtablelist.cshtml" : "TableOutput/detailtable.cshtml",
                  "TableOutput/detailtablehead.cshtml")
        {
        }

        public bool IsFix { get; set; }

        public RazorOutputData OtherNewButton { get; set; }

        protected override object CreateCustomData(bool isEdit)
        {
            if (isEdit)
                return (IsFix: IsFix, OtherNewButton: OtherNewButton);

            return null;
        }
    }
}