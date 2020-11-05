using System.Data;
using System.IO;
using System.Xml;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class BasePostDataSetObjectCreator : IPostObjectCreator
    {
        protected BasePostDataSetObjectCreator()
        {
        }

        #region IPostObjectCreator 成员

        public object Read(IInputData input, Stream stream)
        {
            DataSet ds = new DataSet(ToolkitConst.TOOLKIT) { Locale = ObjectUtil.SysCulture };
            ds.ReadXml(CreateReader(stream));
            TkDebug.ThrowIfNoAppSetting();
            if (BaseAppSetting.Current.IsDebug)
            {
                string file = Path.Combine(BaseAppSetting.Current.XmlPath, "Post.xml");
                FileUtil.WriteFileUseWorkThread(file, ds.GetXml());
            }
            return ds;
        }

        #endregion IPostObjectCreator 成员

        protected abstract XmlReader CreateReader(Stream stream);
    }
}