using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ObjectContainer
    {
        public ObjectContainer(object mainObject)
        {
            MainObject = mainObject;
        }

        public object MainObject { get; private set; }

        public ObjectOperatorCollection Operators { get; private set; }

        public ObjectDecoderContainer Decoder { get; private set; }

        private static IDecoder GetDecoder(IFieldDecoder decoder)
        {
            if (decoder.Type == DecoderType.CodeTable)
            {
                BasePlugInFactory factory = BaseGlobalVariable.Current.FactoryManager.GetCodeFactory(
                    CodeTablePlugInFactory.REG_NAME);
                CodeTable ct = factory.CreateInstance<CodeTable>(decoder.RegName);
                return ct;
            }
            else if (decoder.Type == DecoderType.EasySearch)
            {
                BasePlugInFactory factory = BaseGlobalVariable.Current.FactoryManager.GetCodeFactory(
                    EasySearchPlugInFactory.REG_NAME);
                EasySearch ct = factory.CreateInstance<EasySearch>(decoder.RegName);
                return ct;
            }
            return null;
        }

        public void Decode(IEnumerable<IFieldInfoEx> fields)
        {
            if (fields == null)
                return;
            var decodeFields = from item in fields
                               where item.Decoder != null && item.Decoder.Type != DecoderType.None
                               select item;

            if (Decoder == null)
                Decoder = new ObjectDecoderContainer();
            foreach (var item in decodeFields)
            {
                IDecoder coder = GetDecoder(item.Decoder);
                if (coder != null)
                {
                    IDecoderItem decodeItem = coder.Decode(
                        MainObject.MemberValue(item.NickName).ConvertToString());
                    if (decodeItem != null)
                        Decoder.Add(item.NickName, decodeItem);
                }
            }
        }

        public void SetOperateRight(IPageStyle style, IObjectOperateRight right,
            IEnumerable<string> defaultOperators)
        {
            if (right == null && defaultOperators == null)
                return;

            if (right == null)
                Operators = new ObjectOperatorCollection(defaultOperators);
            else
            {
                TkDebug.AssertArgumentNull(style, "style", this);

                ObjectOperateRightEventArgs e = new ObjectOperateRightEventArgs(style, MainObject);
                Operators = right.GetOperator(e);
            }
        }
    }
}
