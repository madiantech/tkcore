using System.Threading.Tasks;

namespace YJC.Toolkit.Razor
{
    public interface IRazorTemplateCompiler
    {
        ICompilationService CompilationService { get; }

        Task<CompiledTemplateDescriptor> CompileAsync(string templateKey);
    }
}