using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class DerivedUnit : Unit, IDerivedUnit
    {
        /// <summary>
        /// To be called only from <see cref="DerivedQuantity.DerivedQuantity(BaseComposition{BaseQuantity})"/>
        /// </summary>
        /// <param name="quantity"></param>
        internal DerivedUnit(DerivedQuantity quantity)
            : base(null, quantity, 1m)
        {
            MaybeBaseUnitComposition = BaseComposition<IBaseUnit>.
                CreateFromExistingBaseComposition(
                quantity.BaseQuantityComposition,
                baseQuantity => baseQuantity.FundamentalUnit);
        }

        // for defining from an existing IDerivedUnit
        public DerivedUnit(string name, IDerivedUnit otherUnit, decimal multiplier, decimal offset = 0)
            : base(name, otherUnit, multiplier, offset)
        {

        }

        // for defining from a chain of operations
        public DerivedUnit(string name, BaseComposition<IBaseUnit> composition)
            : base(name, composition)
        {

        }
    }
}
