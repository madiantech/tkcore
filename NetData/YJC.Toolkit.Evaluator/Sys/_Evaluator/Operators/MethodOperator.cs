using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YJC.Toolkit.Sys.Evaluator
{
    internal class MethodOperator : Operator<Func<bool, bool, Expression, string, List<Expression>, Expression>>
    {
        public MethodOperator(string value, int precedence, bool leftassoc,
            Func<bool, bool, Expression, string, List<Expression>, Expression> func)
            : base(value, precedence, leftassoc, func)
        {
        }
    }
}