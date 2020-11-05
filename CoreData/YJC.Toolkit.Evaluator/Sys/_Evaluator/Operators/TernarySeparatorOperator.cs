using System;
using System.Linq.Expressions;

namespace YJC.Toolkit.Sys.Evaluator
{
    internal class TernarySeparatorOperator : Operator<Func<Expression, Expression>>
    {
        public TernarySeparatorOperator(string value, int precedence, bool leftassoc,
            Func<Expression, Expression> func)
            : base(value, precedence, leftassoc, func)
        {
        }
    }
}
