using System;
using System.Linq.Expressions;

namespace YJC.Toolkit.Sys.Evaluator
{
    internal class IndexOperator : Operator<Func<Expression, Expression, Expression>>
    {
        public IndexOperator(string value, int precedence, bool leftassoc,
            Func<Expression, Expression, Expression> func)
            : base(value, precedence, leftassoc, func)
        {
        }
    }
}