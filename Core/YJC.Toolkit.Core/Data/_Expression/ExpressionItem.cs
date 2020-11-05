using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal sealed class ExpressionItem : BaseExpressionItem
    {
        private readonly IExpression fExpression;
        private readonly ExpressionAttribute fAttribute;

        public ExpressionItem(IExpression expression, string name)
        {
            fExpression = expression;
            Name = name;
            fAttribute = Attribute.GetCustomAttribute(expression.GetType(),
                typeof(ExpressionAttribute)) as ExpressionAttribute;
            TkDebug.AssertNotNull(fAttribute, string.Format(ObjectUtil.SysCulture,
                "此处错误不该发生，因为插件{0}应该附着了相应的Attribute", expression.GetType()), expression);
            SqlInject = fAttribute.SqlInject;
        }

        public override string Execute(object[] customData)
        {
            try
            {
                GiveExpressionData(fExpression, customData);
                return fExpression.Execute();
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "宏(注册关键字{0})在计算表达式时出错，请检查原因", fAttribute.RegName), ex, fExpression);
                return string.Empty;
            }

        }
    }
}
