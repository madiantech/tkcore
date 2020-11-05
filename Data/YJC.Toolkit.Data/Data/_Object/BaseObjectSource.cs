using System;
using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseObjectSource<T> : IDisposable, ISource, ISupportMetaData
    {
        private IMetaData fMetaData;

        protected BaseObjectSource(T source)
        {
            TkDebug.AssertArgumentNull(source, "source", null);

            Source = source;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ISource 成员

        public abstract OutputData DoAction(IInputData input);

        #endregion

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return UseMetaData;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            fMetaData = metaData;
        }

        #endregion


        public T Source { get; private set; }

        public bool UseMetaData { get; set; }

        public string ObjectName { get; set; }

        protected IEnumerable<IFieldInfoEx> GetFields()
        {
            if (fMetaData == null || string.IsNullOrEmpty(ObjectName))
                return null;

            var scheme = fMetaData.GetTableScheme(ObjectName);
            return scheme == null ? null : scheme.Fields;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Source.DisposeObject();
        }

        protected internal OutputData ErrorPageStyle(PageStyle style, IInputData input)
        {
            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                "当前支持页面类型为{1}，当前类型是{0}", input.Style, style), this);
            return null;
        }
    }
}
