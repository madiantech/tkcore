using System;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class FileCacheInfo : IRegName, IEquatable<FileCacheInfo>, IComparable<FileCacheInfo>
    {
        public FileCacheInfo()
        {
        }

        public FileCacheInfo(string fileName)
        {
            FileInfo info = new FileInfo(fileName);
            FileName = info.FullName;
            LastModified = info.LastWriteTime.Ticks;
        }

        public string RegName { get => FileName; }

        [SimpleAttribute]
        public string FileName { get; private set; }

        [SimpleAttribute]
        public long LastModified { get; private set; }

        public bool IsModified
        {
            get
            {
                FileInfo info = new FileInfo(FileName);
                return info.LastAccessTime.Ticks != LastModified;
            }
        }

        public int CompareTo(FileCacheInfo other)
        {
            if (other == null)
                return 1;
            return FileName.CompareTo(other.FileName);
        }

        public bool Equals(FileCacheInfo other)
        {
            if (other == null)
                return false;
            return FileName == other.FileName && LastModified == other.LastModified;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is FileCacheInfo other))
                return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return FileName.GetHashCode() ^ LastModified.GetHashCode();
        }
    }
}