namespace YJC.Toolkit.Razor
{
    public abstract class TemplatePage<TModel> : TemplatePage
    {
        protected TemplatePage()
            : base()
        {
        }

        public TModel Model { get; set; }

        public override void SetModel(object model)
        {
            Model = (TModel)model;
        }
    }
}