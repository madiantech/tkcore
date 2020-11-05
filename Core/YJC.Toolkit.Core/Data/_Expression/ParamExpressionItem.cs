using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal sealed class ParamExpressionItem : BaseExpressionItem
    {
        private readonly IParamExpression fExpression;
        private readonly ParamExpressionAttribute fAttribute;

        /// <summary>
        /// Initializes a new instance of the ParamExpressionItem class.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="parameter"></param>
        public ParamExpressionItem(IParamExpression expression, string parameter)
        {
            fExpression = expression;
            Name = parameter;
            fAttribute = Attribute.GetCustomAttribute(expression.GetType(),
                typeof(ParamExpressionAttribute)) as ParamExpressionAttribute;
            TkDebug.AssertNotNull(fAttribute, string.Format(ObjectUtil.SysCulture,
                "此处错误不该发生，因为插件{0}应该附着了相应的Attribute", expression.GetType()), expression);
            SqlInject = fAttribute.SqlInject;
        }

        public override string Execute(object[] customData)
        {
            try
            {
                GiveExpressionData(fExpression, customData);
                return fExpression.Execute(Name);
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "参数宏(注册关键字{0})在计算参数为{1}的表达式时出错，请检查原因",
                    fAttribute.RegName, Name), ex, fExpression);
                return string.Empty;
            }
        }
    }
}
