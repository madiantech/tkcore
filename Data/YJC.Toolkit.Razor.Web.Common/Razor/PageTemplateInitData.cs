namespace YJC.Toolkit.Razor
{
    internal class PageTemplateInitData : IPageTemplateInitData
    {
        public PageTemplateInitData(string pageTemplateName, string modelCreatorName)
        {
            PageTemplateName = pageTemplateName;
            ModelCreatorName = modelCreatorName;
        }

        public string PageTemplateName { get; }

        public string ModelCreatorName { get; }
    }
}