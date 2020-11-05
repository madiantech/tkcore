using YJC.Toolkit.Sys;

namespace YJC.Toolkit.HtmlExtension
{
    public class HtmlOption
    {
        public static readonly HtmlOption Default = CreateDefault();

        [SimpleAttribute(DefaultValue = true)]
        public bool Script { get; set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool Link { get; set; }

        [SimpleAttribute]
        public bool Img { get; set; }

        [SimpleAttribute]
        public bool A { get; set; }

        private static HtmlOption CreateDefault()
        {
            return new HtmlOption
            {
                Script = true,
                Link = true
            };
        }
    }
}