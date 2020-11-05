using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    [Serializable]
    public class ImportConvertException : ToolkitException
    {
        public ImportConvertException(string nickName, int row, string message, Exception innerException)
            : base(message, innerException, null)
        {
            NickName = nickName;
            Row = row;
        }

        public string NickName { get; private set; }

        public int Row { get; private set; }
    }
}
