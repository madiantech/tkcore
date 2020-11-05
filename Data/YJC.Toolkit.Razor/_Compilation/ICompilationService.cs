using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;

namespace YJC.Toolkit.Razor
{
    public interface ICompilationService
    {
        CSharpCompilationOptions CSharpCompilationOptions { get; }

        EmitOptions EmitOptions { get; }

        CSharpParseOptions ParseOptions { get; }

        Assembly OperatingAssembly { get; }

        Assembly CompileAndEmit(IGeneratedRazorTemplate razorTemplate);
    }
}