using System;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public sealed class FieldInfoEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the FieldInfoEventArgs class.
        /// </summary>
        internal FieldInfoEventArgs(IFieldInfo fieldInfo, UpdateKind status, SqlPosition position)
        {
            FieldInfo = fieldInfo;
            Status = status;
            Position = position;
        }

        public IFieldInfo FieldInfo { get; private set; }

        public UpdateKind Status { get; private set; }

        public SqlPosition Position { get; set; }
    }
}
