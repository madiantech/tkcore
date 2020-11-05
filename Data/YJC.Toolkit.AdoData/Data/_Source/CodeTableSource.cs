using YJC.Toolkit.Decoder;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class CodeTableSource : BaseDbSource
    {
        public CodeTableSource(string regName)
        {
            TkDebug.AssertArgumentNull(regName, "regName", null);

            RegName = regName;
        }

        public string RegName { get; private set; }

        public override OutputData DoAction(IInputData input)
        {
            CodeTable table = PlugInFactoryManager.CreateInstance<CodeTable>(
                CodeTablePlugInFactory.REG_NAME, RegName);

            table.Fill(DataSet, Context);
            return OutputData.Create(DataSet);
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "注册名为{0}的CodeTable获取其数据的数据源", RegName);
        }
    }
}