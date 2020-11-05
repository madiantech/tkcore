using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ListVueOperator : DetailVueOperator
    {
        public ListVueOperator(string source, List<VueOperator> globalOperators,
            List<VueOperator> rowOperators) : base(source, globalOperators)
        {
            RowOperators = rowOperators;
        }

        [ObjectElement(IsMultiple = true)]
        public List<VueOperator> RowOperators { get; }
    }
}