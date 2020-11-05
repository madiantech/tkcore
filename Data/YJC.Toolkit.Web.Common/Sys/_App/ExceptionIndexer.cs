using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace YJC.Toolkit.Sys
{
    internal class ExceptionIndexer
    {
        private readonly string fFileName;

        public ExceptionIndexer()
        {
            TkDebug.ThrowIfNoAppSetting();

            fFileName = Path.Combine(BaseAppSetting.Current.ErrorPath, "seed.ini");
            if (File.Exists(fFileName))
                try
                {
                    this.ReadXmlFromFile(fFileName);
                }
                catch
                {
                    Index = 0;
                }
            else
                Index = 0;
        }

        [SimpleAttribute]
        internal int Index { get; private set; }

        private void SaveFile()
        {
            string text = this.WriteXml();
            FileUtil.VerifySaveFile(fFileName, text,
                BaseAppSetting.Current.WriteSettings.Encoding);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int NextIndex()
        {
            ++Index;
            if (BaseAppSetting.Current.UseWorkThread)
                BaseGlobalVariable.Current.BeginInvoke(new Action(SaveFile));
            return Index;
        }
    }
}
