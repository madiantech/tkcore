using System;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class RazorCompilerException : Exception, IExceptionInfo
    {
        internal RazorCompilerException(CompilerErrorCollection errors, string source, string compileCode)
            : base(errors[0].ErrorText)
        {
            SourceCode = source;
            CompileCode = compileCode;
            Errors = errors;
        }

        protected RazorCompilerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #region IExceptionInfo 成员

        public void FillExceptionInfo(ExceptionInfo info)
        {
            info.FillInfo(this);
            var otherInfos = info.OtherInfos;
            otherInfos.Add("SourceCode", SourceCode);
            otherInfos.Add("CompileCode", CompileCode);
            int index = 1;
            foreach (CompilerError error in Errors)
            {
                string value = string.Format(ObjectUtil.SysCulture, "行:{0},列:{1},错误:{2}",
                    error.Line, error.Column, error.ErrorText);
                otherInfos.Add("Error" + index++, value);
            }
        }

        #endregion

        public string SourceCode { get; private set; }

        public string CompileCode { get; private set; }

        public CompilerErrorCollection Errors { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
