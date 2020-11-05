namespace YJC.Toolkit.Razor
{
    public interface IGeneratedRazorTemplate
    {
        string TemplateKey { get; }

        string GeneratedCode { get; }

        TkRazorProjectItem ProjectItem { get; set; }
    }
}