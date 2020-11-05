namespace YJC.Toolkit.Xml
{
    public sealed class TransformSetting
    {
        public readonly static TransformSetting Default = new TransformSetting(false, false, false);
        public readonly static TransformSetting Document = new TransformSetting(false, true, true);
        public readonly static TransformSetting InternalXslt = new TransformSetting(true, false, true);
        public readonly static TransformSetting All = new TransformSetting(true, true, true);

        /// <summary>
        /// Initializes a new instance of the TransformSetting class.
        /// </summary>
        public TransformSetting(bool needInternalResolver, bool needEvidence, bool useCache)
        {
            UseCache = useCache;
            NeedEvidence = needEvidence;
            NeedInternalResolver = needInternalResolver;
        }

        public bool UseCache { get; private set; }

        public bool NeedEvidence { get; private set; }

        public bool NeedInternalResolver { get; private set; }
    }
}
