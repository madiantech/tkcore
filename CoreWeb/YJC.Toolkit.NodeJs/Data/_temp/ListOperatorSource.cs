using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class ListOperatorSource : ISource
    {
        public IOperatorsConfig Operators { get; internal set; }

        public OutputData DoAction(IInputData input)
        {
            ListVueOperator result;
            if (Operators?.Operators == null)
                result = new ListVueOperator(input.SourceInfo.Source, null, null);
            else
            {
                var globalOpers = (from item in Operators.Operators
                                   where item.Position == OperatorPosition.Global
                                   select new VueOperator(item)).ToList();
                var rowOpers = (from item in Operators.Operators
                                where item.Position == OperatorPosition.Row
                                select new VueOperator(item)).ToList();
                result = new ListVueOperator(input.SourceInfo.Source, globalOpers, rowOpers);
            }

            return OutputData.CreateToolkitObject(result);
        }
    }
}