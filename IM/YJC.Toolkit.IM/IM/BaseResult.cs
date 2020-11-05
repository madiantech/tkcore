using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public class BaseResult
    {
        [SimpleElement(LocalName = "errcode", Order = 10)]
        public int ErrorCode { get; protected set; }

        [SimpleElement(LocalName = "errmsg", Order = 20)]
        public string ErrorMsg { get; protected set; }

        public bool IsError
        {
            get
            {
                return ErrorCode != 0;
            }
        }

        public void Assign(BaseResult other)
        {
            if (other == null)
                return;

            ErrorCode = other.ErrorCode;
            ErrorMsg = other.ErrorMsg;
        }

        public override string ToString() => ErrorCode == 0 ? ErrorMsg : $"{ErrorCode}:{ErrorMsg}";
    }
}