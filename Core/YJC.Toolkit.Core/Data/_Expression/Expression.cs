using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class Expression
    {
        private readonly ExpressionParser fParser;
        private readonly List<BaseExpressionItem> fExpressions;

        private Expression(string expressionString)
        {
            TkDebug.AssertArgumentNullOrEmpty(expressionString, "expressionString", null);

            fParser = ExpressionParser.ParseExpression(expressionString);
            TkDebug.ThrowIfNoGlobalVariable();
            PlugInFactoryManager factories = BaseGlobalVariable.Current.FactoryManager;
            BasePlugInFactory paramFactory = factories.GetCodeFactory(
                ParamExpressionPlugInFactory.REG_NAME);
            BasePlugInFactory exprFactory = factories.GetCodeFactory(
                ExpressionPlugInFactory.REG_NAME);

            fExpressions = new List<BaseExpressionItem>();

            foreach (string item in fParser.ParamArray)
            {
                string firstChar = item[0].ToString();
                BaseExpressionItem expressionItem;
                if (paramFactory.Contains(firstChar))
                {
                    IParamExpression expression = paramFactory.CreateInstance<IParamExpression>(firstChar);
                    string parameter = item.Substring(1);
                    expressionItem = new ParamExpressionItem(expression, parameter);
                }
                else
                {
                    IExpression expression = exprFactory.CreateInstance<IExpression>(item);
                    expressionItem = new ExpressionItem(expression, item);
                }
                fExpressions.Add(expressionItem);
            }
        }

        private static readonly char[] INJECT_CHARS = new char[] { ';', '<' };

        private string Execute(bool sqlInject, string[] sortedEmptyMarcoes, params object[] customData)
        {
            if (fExpressions.Count == 0)
                return fParser.FormatString;

            string[] values = new string[fExpressions.Count];
            int i = 0;
            foreach (BaseExpressionItem item in fExpressions)
            {
                string value = item.Execute(customData);
                if (string.IsNullOrEmpty(value))
                {
                    if (sortedEmptyMarcoes != null)
                    {
                        if (Array.BinarySearch(sortedEmptyMarcoes, item.Name) >= 0)
                            return string.Empty;
                    }
                    value = value ?? string.Empty;
                }
                else
                {
                    if (sqlInject)
                    {
                        if (item.SqlInject)
                        {
                            if (value.IndexOfAny(INJECT_CHARS) != -1)
                                // 此处不使用TkDebug，当Release版本时，一样需要防止注入
                                throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                                    "由于开启了注入保护，宏{0}中包含了分号或者小于号，这是禁止的，请检查或者关闭注入保护。", item.Name), this);
                            value = value.Replace("\'", "\'\'");
                        }
                    }
                }
                values[i++] = value;
            }
            return string.Format(ObjectUtil.SysCulture, fParser.FormatString, values);
        }

        public static string Execute(string expressionString, params object[] customData)
        {
            TkDebug.AssertArgumentNull(expressionString, "expressionString", null);

            Expression expression = new Expression(expressionString);
            return expression.Execute(true, null, customData);
        }

        public static string Execute(string expressionString, bool sqlInject, params object[] customData)
        {
            TkDebug.AssertArgumentNull(expressionString, "expressionString", null);

            Expression expression = new Expression(expressionString);
            return expression.Execute(sqlInject, null, customData);
        }

        public static string Execute(MarcoConfigItem item, params object[] customData)
        {
            TkDebug.AssertArgumentNull(item, "item", null);

            if (string.IsNullOrEmpty(item.Value))
                return string.Empty;
            if (!item.NeedParse)
                return item.Value;
            Expression expression = new Expression(item.Value);
            string[] emptyMarcoes;
            if (item.EmptyMarcoes != null)
                emptyMarcoes = item.EmptyMarcoes as string[];
            else
                emptyMarcoes = null;
            return expression.Execute(item.SqlInject, emptyMarcoes, customData);
        }
    }
}
