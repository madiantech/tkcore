using System;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public sealed class RazorConfiguration
    {
        public static RazorConfiguration Current { get; internal set; }
        private string fSavePath;

        static RazorConfiguration()
        {
            Current = new RazorConfiguration(true);
        }

        private RazorConfiguration(bool init)
        {
            if (init)
            {
                RaiseOnCompileError = RaiseOnRunError
                    = SaveCompileAssembly = SaveCompileCode = true;

                if (BaseAppSetting.Current != null)
                    SavePath = Path.Combine(BaseAppSetting.Current.XmlPath, @"razor\_temp");
            }
        }

        internal RazorConfiguration(Tuple<bool, bool, bool, bool, string> data)
        {
            RaiseOnCompileError = data.Item1;
            RaiseOnRunError = data.Item2;
            SaveCompileCode = data.Item3;
            SaveCompileAssembly = data.Item4;
            fSavePath = data.Item5;
        }

        public RazorConfiguration()
        {
        }

        public bool RaiseOnCompileError { get; set; }

        public bool RaiseOnRunError { get; set; }

        public bool SaveCompileCode { get; set; }

        public bool SaveCompileAssembly { get; set; }

        public string SavePath
        {
            get
            {
                return fSavePath;
            }
            set
            {
                fSavePath = value;
                if (!string.IsNullOrEmpty(value) && !Directory.Exists(value))
                    Directory.CreateDirectory(value);
            }
        }

        internal Tuple<bool, bool, bool, bool, string> ToTuple()
        {
            return Tuple.Create(RaiseOnCompileError, RaiseOnRunError, SaveCompileCode,
                SaveCompileAssembly, fSavePath);
        }
    }
}
