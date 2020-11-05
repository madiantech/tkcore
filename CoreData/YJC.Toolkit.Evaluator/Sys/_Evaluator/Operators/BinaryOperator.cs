using System;
using System.Linq.Expressions;

namespace YJC.Toolkit.Sys.Evaluator
{
    internal class BinaryOperator : Operator<Func<Expression, Expression, Expression>>
    {
        public BinaryOperator(string value, int precedence, bool leftassoc,
            Func<Expression, Expression, Expression> func, ExpressionType expressionType)
            : base(value, precedence, leftassoc, func)
        {
            Arguments = 2;
            ExpressionType = expressionType;
        }
    }
}