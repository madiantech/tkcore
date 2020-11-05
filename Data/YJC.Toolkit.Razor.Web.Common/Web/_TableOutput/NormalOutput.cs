namespace YJC.Toolkit.Web
{
    public class NormalOutput : RazorTableOutput
    {
        public NormalOutput()
            : base("TableOutput/editnormal.cshtml", "TableOutput/detailnormal.cshtml")
        {
            ColumnCount = 3;
        }

        protected override object CreateCustomData(bool isEdit)
        {
            if (isEdit)
                return (ColumnCount: ColumnCount, Dummy: 0);
            return null;
        }

        public override bool IsSingle => true;

        public int ColumnCount { get; set; }
    }
}