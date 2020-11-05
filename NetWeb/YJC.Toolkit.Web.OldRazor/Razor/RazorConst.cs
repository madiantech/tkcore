using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.Razor
{
    public static class RazorConst
    {
        private static readonly Tuple<Type, Type>[] fNormalTypes;
        private static readonly Tuple<Type, Type>[] fObjectTypes;
        private static readonly Tuple<Type, Type> fCustomType;

        static RazorConst()
        {
            fCustomType = Tuple.Create<Type, Type>(typeof(NormalCustomTemplate), null);
            fNormalTypes = new Tuple<Type, Type>[] {
                Tuple.Create(typeof(NormalListTemplate), typeof(NormalListData)),
                Tuple.Create(typeof(NormalEditTemplate), typeof(NormalEditData)),
                Tuple.Create(typeof(NormalDetailTemplate), typeof(NormalDetailData)),
                Tuple.Create(typeof(NormalDetailListTemplate), typeof(NormalListData)),
                Tuple.Create(typeof(NormalTreeTemplate), typeof(NormalTreeData)),
                Tuple.Create(typeof(NormalTreeDetailTemplate), typeof(NormalDetailData)),
                Tuple.Create(typeof(NormalMultiEditTemplate), typeof(NormalEditData)),
                Tuple.Create(typeof(NormalMultiDetailTemplate), typeof(NormalDetailData))
            };
            fObjectTypes = new Tuple<Type, Type>[] {
                Tuple.Create(typeof(NormalObjectListTemplate), typeof(NormalListData)),
                Tuple.Create(typeof(NormalObjectEditTemplate), typeof(NormalEditData)),
                Tuple.Create(typeof(NormalObjectDetailTemplate), typeof(NormalDetailData)),
                Tuple.Create<Type, Type>(null, null),//Tuple.Create(typeof(NormalDetailListTemplate), typeof(NormalListData)),
                Tuple.Create(typeof(NormalObjectTreeTemplate), typeof(NormalTreeData)),
                Tuple.Create(typeof(NormalObjectTreeDetailTemplate), typeof(NormalDetailData)),
                Tuple.Create<Type, Type>(null, null),//Tuple.Create(typeof(NormalMultiEditTemplate), typeof(NormalEditData)),
                Tuple.Create<Type, Type>(null, null)//Tuple.Create(typeof(NormalMultiDetailTemplate), typeof(NormalDetailData))
            };
        }

        public static Tuple<Type, Type> GetPageDataType(RazorTemplateStyle style, bool isNormal)
        {
            if (style == RazorTemplateStyle.Custom)
                return fCustomType;
            Tuple<Type, Type>[] set = isNormal ? fNormalTypes : fObjectTypes;
            return set[(int)style];
        }
    }
}