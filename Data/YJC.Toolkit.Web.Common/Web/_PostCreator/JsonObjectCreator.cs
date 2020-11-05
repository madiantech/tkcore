using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class JsonObjectCreator : IPostObjectCreator
    {
        private ReadSettings fReadSettings;
        private readonly string fRegClass;

        public JsonObjectCreator(string regClass)
        {
            TkDebug.AssertArgumentNullOrEmpty(regClass, "regClass", null);

            fRegClass = regClass;
            fReadSettings = ObjectUtil.ReadSettings;
        }

        #region IPostObjectCreator 成员

        public object Read(IInputData input, Stream stream)
        {
            object postObject = PlugInFactoryManager.CreateInstance<object>(
                RegClassPlugInFactory.REG_NAME, fRegClass);
            postObject.ReadFromStream("Json", ModelName, stream,
                fReadSettings, QName.ToolkitNoNS);

            return postObject;
        }

        #endregion

        public string ModelName { get; set; }

        public ReadSettings ReadSettings
        {
            get
            {
                return fReadSettings;
            }
            set
            {
                TkDebug.AssertArgumentNull(value, "value", this);

                fReadSettings = value;
            }
        }
    }
}
