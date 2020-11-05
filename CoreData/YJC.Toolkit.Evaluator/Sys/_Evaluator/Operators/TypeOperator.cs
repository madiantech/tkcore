using System;
using System.Linq.Expressions;

namespace YJC.Toolkit.Sys.Evaluator
{
    internal class TypeOperator : Operator<Func<Expression, Type, UnaryExpression>>
    {
        public TypeOperator(string value, int precedence, bool leftassoc,
            Func<Expression, Type, UnaryExpression> func)
            : base(value, precedence, leftassoc, func)
        {
            Arguments = 1;
        }
    }
}