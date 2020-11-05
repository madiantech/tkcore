namespace YJC.Toolkit.Sys
{
    [ExceptionHandlerConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-06-03",
       Author = "YJC", Description = "将配置文件内容输出的Exception处理器")]
    internal class FileExceptionHandlerConfig : IConfigCreator<IExceptionHandler>
    {
        #region IConfigCreator<IExceptionHandler> 成员

        public IExceptionHandler CreateObject(params object[] args)
        {
            string file = FileUtil.GetRealFileName(FileName, Position);
            return new FileExceptionHandler(file) { LogException = Log };
        }

        #endregion IConfigCreator<IExceptionHandler> 成员

        [SimpleAttribute]
        public bool Log { get; internal set; }

        [SimpleAttribute]
        public string FileName { get; private set; }

        [SimpleAttribute(DefaultValue = FilePathPosition.Application)]
        public FilePathPosition Position { get; private set; }
    }
}