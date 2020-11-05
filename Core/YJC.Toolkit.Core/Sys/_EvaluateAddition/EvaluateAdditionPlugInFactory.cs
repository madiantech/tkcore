using System;

namespace YJC.Toolkit.Sys
{
    public class EvaluateAdditionPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_EvaluateAddition";
        private const string DESCRIPTION = "EvaluateAddition插件工厂";

        public EvaluateAdditionPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }

        public void Initialize()
        {
            EnumableCodePlugIn(AddAdditionObj);
        }

        private void AddAdditionObj(string regName, Type type, BasePlugInAttribute attr)
        {
            if (ObjectUtil.IsSubType(typeof(EvaluateAdditionType), type))
            {
                EvaluateAdditionType add = CreateInstance<EvaluateAdditionType>(regName);
                EvaluatorUtil.AddAdditionObj(add.CreateAdditionObject());
            }
            else
            {
                string name = type.Name;
                EvaluatorUtil.AddAdditionObj((name, type));
            }
        }
    }
}