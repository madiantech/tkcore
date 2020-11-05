using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Right;

namespace YJC.Toolkit.Data
{
    public class DetailObjectModel : BaseObjectModel
    {
        private Operator[] fDetailOperators;

        public int DetailOperatorCount
        {
            get
            {
                return fDetailOperators == null ? 0 : fDetailOperators.Length;
            }
        }

        public IEnumerable<Operator> DetailOperators
        {
            get
            {
                return fDetailOperators;
            }
            set
            {
                if (value != null)
                    fDetailOperators = value.ToArray();
                else
                    fDetailOperators = null;
            }
        }
    }
}
