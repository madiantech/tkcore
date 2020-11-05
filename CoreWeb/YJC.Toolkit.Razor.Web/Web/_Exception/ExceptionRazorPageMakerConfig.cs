using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-08-17",
        Description = @"使用Normal/Bin/Exception.cshtml输出Get的错误，发送文件Id输出Post的错误的PageMaker(Exception专用)")]
    internal class ExceptionRazorPageMakerConfig : IConfigCreator<IPageMaker>
    {
        private const string PART_FILE_NAME = "Bin/Exception.cshtml";
        private const string FILE_NAME = @"^Normal/" + PART_FILE_NAME;

        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.QueryObject<IInputData>(args);
            TkDebug.AssertNotNull(input, "参数args中缺少IInputData类型", this);

            if (input.IsPost)
                return new ExceptionPostPageMaker();
            else
            {
                string fileName = UseTemplate ? WebRazorUtil.GetTemplateFile(PART_FILE_NAME) : FILE_NAME;
                return new FreeRazorPageMaker(fileName);
            }
        }

        #endregion IConfigCreator<IPageMaker> 成员

        [SimpleAttribute]
        public bool UseTemplate { get; set; }
    }
}