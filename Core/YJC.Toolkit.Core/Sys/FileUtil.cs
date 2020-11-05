using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public static class FileUtil
    {
        private const int BUFFER_SIZE = 10 * 1024;
        private static readonly IEnumerable<string> Empty = Enumerable.Empty<string>();

        public static string GetRealFileName(string fileName, FilePathPosition position)
        {
            TkDebug.ThrowIfNoAppSetting();
            switch (position)
            {
                case FilePathPosition.Application:
                    fileName = Path.Combine(BaseAppSetting.Current.AppPath, fileName);
                    break;

                case FilePathPosition.Error:
                    fileName = Path.Combine(BaseAppSetting.Current.ErrorPath, fileName);
                    break;

                case FilePathPosition.Solution:
                    fileName = Path.Combine(BaseAppSetting.Current.SolutionPath, fileName);
                    break;

                case FilePathPosition.Xml:
                    fileName = Path.Combine(BaseAppSetting.Current.XmlPath, fileName);
                    break;
            }
            return fileName;
        }

        public static string GetFileName(Assembly assembly)
        {
            TkDebug.AssertArgumentNull(assembly, "assembly", null);
            try
            {
                return new Uri(assembly.CodeBase).AbsolutePath;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 将数据保存到相应的文件中
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">需要保存的数据</param>
        /// <param name="encoding">文件的编码形式</param>
        public static void SaveFile(string fileName, string content, Encoding encoding)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);
            TkDebug.AssertArgumentNull(content, "content", null);
            TkDebug.AssertArgumentNull(encoding, "encoding", null);

            FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            using (TextWriter writer = new StreamWriter(file, encoding))
            {
                writer.Write(content);
            }
        }

        public static void SaveFile(string fileName, string content)
        {
            SaveFile(fileName, content, Encoding.Default);
        }

        public static void ConfirmPath(string path)
        {
            TkDebug.AssertArgumentNullOrEmpty(path, "path", null);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void VerifySaveFile(string fileName, string content, Encoding encoding)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            string path = Path.GetDirectoryName(fileName);
            ConfirmPath(path);
            SaveFile(fileName, content, encoding);
        }

        public static void VerifySaveFile(string fileName, string content)
        {
            VerifySaveFile(fileName, content, Encoding.Default);
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            int length;
            while ((length = input.Read(buffer, 0, BUFFER_SIZE)) > 0)
            {
                output.Write(buffer, 0, length);
            }
            output.Flush();
        }

        internal static string GetXmlFilePattern(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return "*.xml";
            TkDebug.Assert(pattern.EndsWith(".xml", StringComparison.OrdinalIgnoreCase),
                string.Format(ObjectUtil.SysCulture, "{0}应该是以.xml结束，而现在不是", pattern), null);
            return pattern;
        }

        public static string JoinPath(string path1, string path2)
        {
            TkDebug.AssertArgumentNullOrEmpty(path1, nameof(path1), null);
            TkDebug.AssertArgumentNullOrEmpty(path2, nameof(path2), null);

            string[] path1Arr = path1.Split('/', '\\');
            Stack<string> pathStack = new Stack<string>();
            // 最后一个文件名不用push到堆栈中
            for (int i = 0; i < path1Arr.Length - 1; ++i)
                pathStack.Push(path1Arr[i]);

            string[] path2Arr = path2.Split('/', '\\');
            bool stackEmpty = false;
            foreach (string item in path2Arr)
            {
                if (item == ".")
                    continue;
                if (item == "..")
                {
                    if (!stackEmpty)
                    {
                        if (pathStack.Count == 0)
                            stackEmpty = true;
                        else
                            pathStack.Pop();
                    }
                    if (stackEmpty)
                        pathStack.Push(item);
                }
                else
                    pathStack.Push(item);
            }
            return string.Join("/", pathStack.Reverse());
        }

        public static void WriteFileUseWorkThread(string fileName, string content)
        {
            TkDebug.ThrowIfNoAppSetting();
            if (!BaseAppSetting.Current.UseWorkThread)
                return;

            TkDebug.ThrowIfNoGlobalVariable();

            BaseGlobalVariable.Current.BeginInvoke(new Action<string, string>(FileUtil.VerifySaveFile),
                fileName, content);
        }

        public static IEnumerable<string> GetFileDependecy(object obj)
        {
            if (obj == null)
                return Empty;
            if (obj is IFileDependency dependency)
            {
                var result = dependency.Files;
                if (result != null)
                    return result;
            }
            return Empty;
        }

        public static IEnumerable<string> GetFileDependecyFromEnumerable(IEnumerable<object> enumerable)
        {
            if (enumerable == null)
                return Empty;
            var files = from item in enumerable
                        let depend = item as IFileDependency
                        where depend != null
                        select depend.Files;
            var result = from item in files
                         from file in item
                         select file;
            return result;
        }
    }
}