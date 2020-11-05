using System.Linq.Expressions;

namespace YJC.Toolkit.Sys.Evaluator
{
    internal abstract class Operator<T> : IOperator
    {
        protected Operator(string value, int precedence, bool leftassoc, T func)
        {
            Value = value;
            Precedence = precedence;
            LeftAssoc = leftassoc;
            Func = func;
        }

        protected Operator(string value, int precedence, bool leftassoc,
            T func, ExpressionType expressionType)
        {
            Value = value;
            Precedence = precedence;
            LeftAssoc = leftassoc;
            Func = func;
            ExpressionType = expressionType;
        }

        public T Func { get; set; }

        public string Value { get; set; }

        public int Precedence { get; set; }

        public int Arguments { get; set; }

        public bool LeftAssoc { get; set; }

        public ExpressionType ExpressionType { get; set; }

        public virtual T GetFunc()
        {
            return Func;
        }

    }
}
