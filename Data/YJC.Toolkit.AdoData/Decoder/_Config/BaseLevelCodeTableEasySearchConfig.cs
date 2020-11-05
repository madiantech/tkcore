using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    abstract class BaseLevelCodeTableEasySearchConfig : BaseCodeTableEasySearchConfig
    {
        protected BaseLevelCodeTableEasySearchConfig()
        {
        }

        [SimpleElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Level", Required = true)]
        public List<int> Levels { get; protected set; }

        public LevelTreeDefinition TreeDefinition
        {
            get
            {
                return new LevelTreeDefinition(null, null, Levels);
            }
        }
    }
}
