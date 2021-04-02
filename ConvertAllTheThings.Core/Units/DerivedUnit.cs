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
        /// To be called only from <see cref="DerivedQuantity.DerivedQuantity(NamedComposition{BaseQuantity})"/>
        /// </summary>
        /// <param name="quantity"></param>
        internal DerivedUnit(DerivedQuantity quantity)
            : base(null, quantity, 1m, GetComposition(quantity))
        {

        }

        private static NamedComposition<IUnit> GetComposition(DerivedQuantity quant)
        {
            return NamedComposition<IUnit>.
                CreateFromExistingBaseComposition(
                quant.BaseQuantityComposition,
                baseQuantity => baseQuantity.FundamentalUnit);
        }

        // for defining from an existing IDerivedUnit
        public DerivedUnit(string name, IDerivedUnit otherUnit, decimal multiplier, decimal offset = 0)
            : base(name, otherUnit, multiplier, offset)
        {

        }

        // for defining from a chain of operations
        internal DerivedUnit(string name, NamedComposition<IUnit> composition)
            : base(name, composition)
        {

        }
    }
}
