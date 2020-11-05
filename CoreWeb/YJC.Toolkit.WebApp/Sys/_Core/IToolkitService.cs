using System;
using System.Collections.Generic;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public interface IToolkitService
    {
        WebGlobalVariable GlobalVariable { get; }

        WebAppSetting AppSetting { get; }
    }
}