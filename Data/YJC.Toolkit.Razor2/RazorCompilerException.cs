using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class RazorCompilerException : Exception, IExceptionInfo
    {
        internal RazorCompilerException(List<Diagnostic> errors, string templateKey, string compileCode)
            : base(errors[0].GetMessage())
        {
            TemplateKey = templateKey;
            CompileCode = compileCode;
            Errors = errors;
        }

        public void FillExceptionInfo(ExceptionInfo info)
        {
            info.FillInfo(this);
            var otherInfos = info.OtherInfos;
            otherInfos.Add(nameof(TemplateKey), TemplateKey);
            int index = 1;
            foreach (var diagnostic in Errors)
            {
                FileLinePositionSpan lineSpan = diagnostic.Location.SourceTree.GetMappedLineSpan(
                    diagnostic.Location.SourceSpan);
                string errorMessage = diagnostic.GetMessage();
                var pos = lineSpan.StartLinePosition;
                string formattedMessage = $"行:{pos.Line + 1},列:{pos.Character + 1},错误:{errorMessage}";

                otherInfos.Add($"Error{index++}", formattedMessage);
            }
            otherInfos.Add(nameof(CompileCode), CompileCode);
        }

        public string TemplateKey { get; }

        public string CompileCode { get; }

        public List<Diagnostic> Errors { get; private set; }
    }
}