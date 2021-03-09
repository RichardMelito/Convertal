using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class EmptyQuantity : IBaseQuantity
    {
        public Unit FundamentalUnit => throw new NotImplementedException();

        public BaseComposition<IBaseQuantity> BaseQuantityComposition => throw new NotImplementedException();

        public string Name => "";

        public string NameSpace => "";

        public string FullName => "";

        public int CompareTo(INamed? other)
        {
            if (Equals(other))
                return 0;

            return 1;
        }
    }
}
