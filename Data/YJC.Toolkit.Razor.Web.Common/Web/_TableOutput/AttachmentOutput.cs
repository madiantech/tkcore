namespace YJC.Toolkit.Web
{
    internal class AttachmentOutput : RazorTableOutput
    {
        public AttachmentOutput()
            : this(false)
        {
        }

        public AttachmentOutput(bool directShowDetail)
            : base("TableOutput/editattachment.cshtml",
                  directShowDetail ? "TableOutput/detailattachmentlist.cshtml" : "TableOutput/detailtable.cshtml",
                  "TableOutput/detailtablehead.cshtml", "TableOutput/detailattachment.cshtml")
        {
        }

        public bool IsMultiple { get; set; }

        protected override object CreateCustomData(bool isEdit)
        {
            if (isEdit)
                return (Multiple: IsMultiple, Dummy: 0);

            return null;
        }
    }
}