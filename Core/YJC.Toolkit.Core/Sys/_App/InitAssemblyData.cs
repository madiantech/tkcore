namespace YJC.Toolkit.Sys
{
    internal sealed class InitAssemblyData
    {
        private readonly InitializationAttribute fAttribute;

        public InitAssemblyData(InitializationAttribute attribute)
        {
            fAttribute = attribute;
            Initialization = ObjectUtil.CreateObject(
                fAttribute.InitClassType) as IInitialization;
        }

        public IInitialization Initialization { get; private set; }

        public InitPriority Priorty
        {
            get
            {
                return fAttribute.Priority;
            }
        }
    }
}
