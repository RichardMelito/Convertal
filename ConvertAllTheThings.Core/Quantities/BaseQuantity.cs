using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class BaseQuantity : Quantity, IBase, IEquatable<BaseQuantity>
    {
        public override Unit FundamentalUnit => throw new NotImplementedException();



        public bool Equals(BaseQuantity? other)
        {
            return base.Equals(other);
        }
    }
}
