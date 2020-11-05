using System;

namespace YJC.Toolkit.Sys
{
    [Serializable]
    public class RedirectException : Exception
    {
        public RedirectException(string page)
        {
            Url = new Uri(page, UriKind.RelativeOrAbsolute);
        }

        //[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
        //public override void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    base.GetObjectData(info, context);
        //}

        public Uri Url { get; private set; }
    }
}
