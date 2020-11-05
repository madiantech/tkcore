using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal static class EsModelUtil
    {
        private const string ManifestName = "manifest.json";
        public static readonly string EsTempPath;
        public static readonly string EsTemplatePath;
        public static readonly string RazorTemplatePath;

        static EsModelUtil()
        {
            TkDebug.ThrowIfNoAppSetting();

            EsTempPath = Path.Combine(BaseAppSetting.Current.XmlPath, "estemp");
            EsTemplatePath = Path.Combine(BaseAppSetting.Current.XmlPath, "estemplate");
            RazorTemplatePath = Path.Combine(BaseAppSetting.Current.XmlPath, "razortemplate");
        }

        public static void Execute(string workDir, string argument)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("node")
            {
                Arguments = argument,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = workDir
            };
            using (Process process = new Process() { StartInfo = startInfo })
            {
                process.Start();
                TkTrace.LogInfo("Node正在执行中");
                process.WaitForExit();
            }
        }

        public static string GetDestPath(IEsModel model, PageSourceInfo info)
        {
            return Path.Combine(EsTempPath, model.NodeDirectory, "Modules", info.ModuleCreator, info.Source);
        }

        private static string GetModelPath(IEsModel model, IEsTemplate template)
        {
            return Path.Combine(EsTemplatePath, model.FileDirectory, template.TemplatePath);
        }

        public static void DeepCopy(string sourcePath, string destPath)
        {
            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);
            if (!Directory.Exists(sourcePath))
                Directory.CreateDirectory(sourcePath);

            string[] files = Directory.GetFiles(sourcePath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                File.Copy(file, Path.Combine(destPath, fileName), true);
            }

            string[] dirs = Directory.GetDirectories(sourcePath);
            foreach (string dir in dirs)
            {
                string newDir = Path.GetRelativePath(sourcePath, dir);
                //string newDir = dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                DeepCopy(dir, Path.Combine(destPath, newDir));
            }
        }

        public static string Execute(IEsModel model, IEsTemplate template,
            HttpContext context, PageSourceInfo info)
        {
            IModule module = info.CreateModule();
            string srcPath = GetModelPath(model, template);
            string dstPath = GetDestPath(model, info);
            int destVersion = TryGetVersion(dstPath);
            int modVersion = EsModelSettings.Current.GetVersion(model, template);
            if (destVersion == -1 || destVersion != modVersion)
            {
                TkTrace.LogInfo($"复制文件到{dstPath}");
                DeepCopy(srcPath, dstPath);
                WriteModelVersion(dstPath, modVersion);
            }
            var dependFiles = template.Generate(model, context, info, module);
            template.ExecuteNode(dstPath);
            string jsFileName = Path.Combine(dstPath, template.BundleFileName);
            string js = File.ReadAllText(jsFileName);

            IEnumerable<string> allDepend = EnumUtil.Convert(dependFiles,
                FileUtil.GetFileDependecy(module)).Distinct();
            var cacheFiles = (from item in allDepend
                              select new FileCacheInfo(item)).ToList();
            JsCacheUtil.Save(info, js, model, template, modVersion, cacheFiles);

            return js;
        }

        public static int TryGetVersion(string path)
        {
            if (!Directory.Exists(path))
                return -1;

            string fileName = Path.Combine(path, ManifestName);
            try
            {
                string json = File.ReadAllText(fileName, Encoding.UTF8);
                EsModelInfo info = new EsModelInfo();
                info.ReadJson(json);

                return info.Version;
            }
            catch
            {
                return -1;
            }
        }

        private static void WriteModelVersion(string path, int version)
        {
            string fileName = Path.Combine(path, ManifestName);
            EsModelInfo info = new EsModelInfo { Version = version };
            FileUtil.VerifySaveFile(fileName, info.WriteJson(), Encoding.UTF8);
        }

        public static List<FileCacheInfo> CreateModelCache(string path)
        {
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            List<FileCacheInfo> result = new List<FileCacheInfo>();
            foreach (var file in files)
            {
                var info = new FileCacheInfo(file);
                result.Add(info);
            }
            result.Sort();
            return result;
        }
    }
}