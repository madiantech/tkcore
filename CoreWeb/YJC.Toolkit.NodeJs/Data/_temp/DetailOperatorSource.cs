using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class DetailOperatorSource : ISource
    {
        public IOperatorsConfig Operators { get; internal set; }

        public OutputData DoAction(IInputData input)
        {
            DetailVueOperator result;
            if (Operators?.Operators == null)
                result = new DetailVueOperator(input.SourceInfo.Source, null);
            else
            {
                var globalOpers = (from item in Operators.Operators
                                   select new VueOperator(item)).ToList();
                result = new DetailVueOperator(input.SourceInfo.Source, globalOpers);
            }

            return OutputData.CreateToolkitObject(result);
        }
    }
}