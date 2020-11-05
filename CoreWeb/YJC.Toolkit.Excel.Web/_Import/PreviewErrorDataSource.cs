using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Excel
{
    internal class PreviewErrorDataSource : ISource
    {
        public OutputData DoAction(IInputData input)
        {
            return OutputData.Create("");
        }
    }
}
