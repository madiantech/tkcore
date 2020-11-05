using System;
using System.Collections.Generic;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class CompositeCreator : IPostObjectCreator
    {
        private class CreatorInfo : IOrder
        {
            /// <summary>
            /// Initializes a new instance of the PageMakerInfo class.
            /// </summary>
            public CreatorInfo(Func<IInputData, bool> function,
                IConfigCreator<IPostObjectCreator> creator, int order)
            {
                Function = function;
                Creator = creator;
                Order = order;
            }

            public Func<IInputData, bool> Function { get; private set; }

            public IConfigCreator<IPostObjectCreator> Creator { get; private set; }

            public int Order { get; private set; }
        }

        private readonly List<CreatorInfo> fList = new List<CreatorInfo>();

        public CompositeCreator(CompositeCreatorConfig config)
        {
            if (config.Items != null)
                foreach (var item in config.Items)
                    fList.Add(new CreatorInfo(item.Condition.UseCondition, item.Creator, 0));
        }

        #region IPostObjectCreator 成员

        public object Read(IInputData input, Stream stream)
        {
            foreach (CreatorInfo item in fList)
            {
                if (item.Function(input))
                {
                    IPostObjectCreator source = item.Creator.CreateObject(input);
                    if (source == null)
                        return null;
                    else
                        return source.Read(input, stream);
                }
            }
            TkDebug.ThrowToolkitException("当前的状态下，没有一个注册的PostObjectCreator满足其条件，请检查条件", this);
            return null;
        }

        #endregion
    }
}
