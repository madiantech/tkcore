using System;

namespace YJC.Toolkit.Sys
{
    internal class ApplicationConfigItem : BaseApplicationConfigItem
    {
        [SimpleAttribute]
        public bool Single { get; private set; }
      
        [SimpleAttribute]
        public bool UseWorkThread { get; private set; }
    }
}