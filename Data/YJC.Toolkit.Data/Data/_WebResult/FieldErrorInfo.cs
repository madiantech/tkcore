using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class FieldErrorInfo
    {
        public FieldErrorInfo()
        {
        }

        public FieldErrorInfo(string tableName, string nickName, string message)
            : this(tableName, nickName, message, 0)
        {
        }

        public FieldErrorInfo(string tableName, string nickName, string message, int position)
        {
            TableName = tableName;
            NickName = nickName;
            Message = message;
            Position = position;
        }

        [SimpleAttribute]
        public string TableName { get; set; }

        [SimpleAttribute]
        public string NickName { get; set; }

        [SimpleAttribute]
        public string Message { get; set; }

        [SimpleAttribute]
        public int Position { get; set; }
    }
}
