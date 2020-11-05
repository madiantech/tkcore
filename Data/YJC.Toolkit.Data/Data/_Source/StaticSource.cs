using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class StaticSource : ISource
    {
        protected StaticSource()
        {
        }

        public StaticSource(string content)
            : this()
        {
            Content = content;
        }

        internal StaticSource(StaticSourceConfig config)
        {
            Content = config.Content;
        }

        internal StaticSource(MarcoSourceConfig config)
        {
            Content = Expression.Execute(config);
        }

        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            return OutputData.Create(Content);
        }

        #endregion

        public string Content { get; protected set; }
    }
}
