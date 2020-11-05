using System;
using System.Linq.Expressions;

namespace YJC.Toolkit.Sys.Evaluator
{
    internal class TernaryOperator : Operator<Func<Expression, Expression, Expression, Expression>>
    {
        public TernaryOperator(string value, int precedence, bool leftassoc,
                              Func<Expression, Expression, Expression, Expression> func)
            : base(value, precedence, leftassoc, func)
        {
            Arguments = 3;
        }
    }
}
