namespace YJC.Toolkit.Data
{
    internal abstract class BaseExpressionItem
    {
        internal BaseExpressionItem()
        {
        }

        public string Name { get; protected set; }

        public bool SqlInject { get; protected set; }

        public abstract string Execute(object[] customData);

        protected static void GiveExpressionData(object expression, object[] customData)
        {
            ICustomData needData = expression as ICustomData;
            if (needData != null)
                needData.SetData(customData);
        }
    }
}
