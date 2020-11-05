using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YJC.Toolkit.Sys.Evaluator
{
    internal class OpFuncServiceLocator
    {
        private static readonly OpFuncServiceLocator Instance = new OpFuncServiceLocator();

        private readonly Dictionary<Type, Func<OpFuncArgs, Expression>>
            fTypeActions = new Dictionary<Type, Func<OpFuncArgs, Expression>>();

        private OpFuncServiceLocator()
        {
            fTypeActions.Add(typeof(MethodOperator), OpFuncServiceProviders.MethodOperatorFunc);
            fTypeActions.Add(typeof(TypeOperator), OpFuncServiceProviders.TypeOperatorFunc);
            fTypeActions.Add(typeof(UnaryOperator), OpFuncServiceProviders.UnaryOperatorFunc);
            fTypeActions.Add(typeof(BinaryOperator), OpFuncServiceProviders.BinaryOperatorFunc);
            fTypeActions.Add(typeof(TernaryOperator), OpFuncServiceProviders.TernaryOperatorFunc);
            fTypeActions.Add(typeof(TernarySeparatorOperator), OpFuncServiceProviders.TernarySeparatorOperatorFunc);

        }

        public static Func<OpFuncArgs, Expression> Resolve(Type type)
        {
            return Instance.ResolveType(type);
        }

        private Func<OpFuncArgs, Expression> ResolveType(Type type)
        {
            return fTypeActions[type];
        }
    }
}