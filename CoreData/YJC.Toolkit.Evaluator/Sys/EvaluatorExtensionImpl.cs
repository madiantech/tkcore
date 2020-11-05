using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace YJC.Toolkit.Sys
{
    internal class EvaluatorExtensionImpl : IEvaluatorExtension
    {
        public Func<object, object> BuildDynamicGetter(Type targetType, string propertyName)
        {
            var rootParam = Expression.Parameter(typeof(object));
            var propBinder = Binder.GetMember(CSharpBinderFlags.None, propertyName, targetType,
                new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
            DynamicExpression propGetExpression = Expression.Dynamic(propBinder, typeof(object),
                Expression.Convert(rootParam, targetType));
            Expression<Func<object, object>> getPropExpression =
                Expression.Lambda<Func<object, object>>(propGetExpression, rootParam);
            return getPropExpression.Compile();
        }

        public object Execute(string expression, params (string Name, object Value)[] additionObjs)
        {
            TkDebug.AssertArgumentNullOrEmpty(expression, "expression", null);

            var expr = new CompiledExpression(expression);
            expr.TypeRegistry.RegisterDefaultTypes();
            foreach (var item in EvaluatorUtil.AdditionObj)
                expr.TypeRegistry.Add(item.Key, item.Value);

            if (additionObjs != null)
                foreach (var item in additionObjs)
                {
                    expr.TypeRegistry.Add(item.Name, item.Value);
                }

            return expr.Eval();
        }
    }
}