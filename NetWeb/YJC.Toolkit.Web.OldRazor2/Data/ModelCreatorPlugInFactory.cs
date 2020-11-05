using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ModelCreatorPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_ModelCreator";
        private const string DESCRIPTION = "ModelCreator插件工厂";

        public ModelCreatorPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}