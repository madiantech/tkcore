using System.Linq.Expressions;

namespace YJC.Toolkit.Sys.Evaluator
{
    internal interface IOperator
    {
        string Value { get; set; }

        int Precedence { get; set; }

        int Arguments { get; set; }

        bool LeftAssoc { get; set; }

        ExpressionType ExpressionType { get; set; }
    }
}