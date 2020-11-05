using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class BaseEsModel : IEsModel
    {
        private readonly Dictionary<string, ISinglePageGenerator> fDictionary;
        private BasePlugInAttribute fAttribute;

        protected BaseEsModel(string fileDirectory, string nodeDirectory)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileDirectory, nameof(fileDirectory), null);
            TkDebug.AssertArgumentNullOrEmpty(nodeDirectory, nameof(nodeDirectory), null);

            FileDirectory = fileDirectory;
            NodeDirectory = nodeDirectory;
            fDictionary = new Dictionary<string, ISinglePageGenerator>();
        }

        public string FileDirectory { get; }

        public virtual string Name { get => Attribute.GetRegName(GetType()); }

        public string NodeDirectory { get; }

        public virtual BasePlugInAttribute Attribute
        {
            get
            {
                if (fAttribute == null)
                {
                    fAttribute = System.Attribute.GetCustomAttribute(GetType(),
                        typeof(EsModelAttribute)).Convert<EsModelAttribute>();
                }
                return fAttribute;
            }
        }

        public ISinglePageGenerator GetPageGenerator(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, nameof(name), this);

            return ObjectUtil.TryGetValue(fDictionary, name);
        }

        public void Add(string name, ISinglePageGenerator generator)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, nameof(name), this);
            TkDebug.AssertArgumentNull(generator, nameof(generator), this);
            TkDebug.AssertArgument(!fDictionary.ContainsKey(name), nameof(name),
                $"已存在Key为{name}的SinglePageGenerator", this);

            fDictionary.Add(name, generator);
        }

        public override string ToString() => $"路径为{FileDirectory}的EsModel";
    }
}