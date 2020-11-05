using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ResolverConfig : IDisposable, IRegName
    {
        private readonly bool fDisposeResolver;

        public ResolverConfig(IConfigCreator<TableResolver> resolverConfig, IDbDataSource source,
            PageStyle style, UpdateKind kind, UpdateMode mode)
            : this(CreateTableResolver(resolverConfig, source), style, kind, mode, true)
        {
        }

        public ResolverConfig(IConfigCreator<TableResolver> resolverConfig, IDbDataSource source,
            PageStyle style, UpdateKind kind, UpdateMode mode, bool disposeResolver)
            : this(CreateTableResolver(resolverConfig, source), style, kind, mode, disposeResolver)
        {
        }

        public ResolverConfig(TableResolver resolver, PageStyle style, UpdateKind kind,
            UpdateMode mode)
            : this(resolver, style, kind, mode, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ResolverConfig class.
        /// </summary>
        public ResolverConfig(TableResolver resolver, PageStyle style, UpdateKind kind,
            UpdateMode mode, bool disposeResolver)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", this);

            Resolver = resolver;
            resolver.UpdateMode = mode;
            Kind = kind;
            Mode = mode;
            Style = style;
            fDisposeResolver = disposeResolver;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (fDisposeResolver)
                Resolver.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        private static TableResolver CreateTableResolver(IConfigCreator<TableResolver> resolverConfig,
            IDbDataSource source)
        {
            TableResolver resolver = resolverConfig.CreateObject(source);
            return resolver;
        }

        public TableResolver Resolver { get; private set; }

        public UpdateKind Kind { get; private set; }

        public UpdateMode Mode { get; private set; }

        public PageStyle Style { get; private set; }

        public string RegName { get => Resolver.TableName; }

        public bool UseMeta { get; set; } = true;
    }
}