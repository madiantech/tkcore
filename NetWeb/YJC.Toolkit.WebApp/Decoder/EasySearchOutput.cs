using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class EasySearchOutput
    {
        /// <summary>
        /// Initializes a new instance of the EasySearchOutput class.
        /// </summary>
        public EasySearchOutput(IEnumerable<IDecoderItem> easySearchItems)
        {
            EasySearchItems = easySearchItems;
            //if (easySearchItems != null)
            //{

            //}
        }

        [ObjectElement(IsMultiple = true, ObjectType = typeof(DataRowDecoderItem), LocalName = "EasySearch")]
        public IEnumerable<IDecoderItem> EasySearchItems { get; private set; }
    }
}
