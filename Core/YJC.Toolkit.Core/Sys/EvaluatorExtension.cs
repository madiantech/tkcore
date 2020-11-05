using System;
using System.Collections.Generic;
using System.Text;

namespace YJC.Toolkit.Sys
{
    internal static class EvaluatorExtension
    {
        private static IEvaluatorExtension fExtension;

        public static IEvaluatorExtension Extension => fExtension;

        public static void SetExtension(IEvaluatorExtension extension)
        {
            fExtension = extension;
        }
    }
}