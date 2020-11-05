using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite
{
    public class InternalFieldControl : IFieldControl
    {
        [SimpleAttribute]
        public ControlType Control { get; set; }

        [SimpleAttribute]
        public int Order { get; set; }

        public PageStyle DefaultShow { get => PageStyle.All; }

        public ControlType GetControl(IPageStyle style)
        {
            return Control;
        }

        public Tuple<string, string> GetCustomControl(IPageStyle style)
        {
            return null;
        }

        public int GetOrder(IPageStyle style)
        {
            return Order;
        }
    }
}