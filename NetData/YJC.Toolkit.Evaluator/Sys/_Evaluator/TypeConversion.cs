using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YJC.Toolkit.Sys
{
    internal class TypeConversion
    {
        private readonly Dictionary<Type, int> fTypePrecedence = null;
        private static readonly TypeConversion Instance = new TypeConversion();

        private TypeConversion()
        {
            fTypePrecedence = new Dictionary<Type, int> 
            {
                {typeof (object), 0},
                {typeof (bool), 1},
                {typeof (byte), 2},
                {typeof (int), 3},
                {typeof (short), 4},
                {typeof (long), 5},
                {typeof (float), 6},
                {typeof (double), 7}
            };
        }

        /// <summary>
        /// Performs implicit conversion between two expressions depending on their type precedence
        /// </summary>
        /// <param name="le"></param>
        /// <param name="re"></param>
        internal static void Convert(ref Expression le, ref Expression re)
        {
            if (Instance.fTypePrecedence.ContainsKey(le.Type)
                && Instance.fTypePrecedence.ContainsKey(re.Type))
            {
                if (Instance.fTypePrecedence[le.Type] > Instance.fTypePrecedence[re.Type])
                    re = Expression.Convert(re, le.Type);
                if (Instance.fTypePrecedence[le.Type] < Instance.fTypePrecedence[re.Type])
                    le = Expression.Convert(le, re.Type);
            }
        }

        /// <summary>
        /// Performs implicit conversion on an expression against a specified type
        /// </summary>
        /// <param name="le"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Expression Convert(Expression le, Type type)
        {
            if (Instance.fTypePrecedence.ContainsKey(le.Type)
                && Instance.fTypePrecedence.ContainsKey(type))
            {
                if (Instance.fTypePrecedence[le.Type] < Instance.fTypePrecedence[type])
                    return Expression.Convert(le, type);
            }
            return le;
        }

        /// <summary>
        /// Compares two types for implicit conversion
        /// </summary>
        /// <param name="from">The source type</param>
        /// <param name="to">The destination type</param>
        /// <returns>-1 if conversion is not possible, 0 if no conversion necessary, +1 if conversion possible</returns>
        internal static int CanConvert(Type from, Type to)
        {
            if (Instance.fTypePrecedence.ContainsKey(from)
                && Instance.fTypePrecedence.ContainsKey(to))
            {
                return Instance.fTypePrecedence[to] - Instance.fTypePrecedence[from];
            }
            else
            {
                if (from == to) return 0;
                if (to.IsAssignableFrom(from)) return 1;
            }
            return -1;
        }
    }
}