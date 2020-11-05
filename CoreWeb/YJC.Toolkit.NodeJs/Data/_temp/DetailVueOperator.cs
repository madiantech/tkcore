using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DetailVueOperator
    {
        public DetailVueOperator(string source, List<VueOperator> globalOperators)
        {
            Source = source;
            GlobalOperators = globalOperators;
        }

        [SimpleAttribute]
        public string Source { get; }

        [ObjectElement(IsMultiple = true)]
        public List<VueOperator> GlobalOperators { get; }
    }
}