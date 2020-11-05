using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class NoFunctionRightException : ErrorPageException
    {
        public NoFunctionRightException()
            : base("权限不够！", "对不起，您的权限不够，无法使用该功能。")
        //: base(TkWebApp.NoFunctionRightTitle, TkWebApp.NoFunctionRightBody)
        {
        }
    }
}