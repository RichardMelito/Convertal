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
        internal DerivedUnit(Database database, DerivedQuantity quantity)
            : base(database, null, quantity, 1m, GetComposition(quantity))
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
        internal DerivedUnit(
            Database database,
            string name, 
            IDerivedUnit otherUnit, 
            decimal multiplier, 
            decimal offset = 0,
            string? symbol = null)
            : base(database, name, otherUnit, multiplier, offset, symbol: symbol)
        {

        }

        // for defining from a chain of operations
        internal DerivedUnit(Database database, string name, NamedComposition<IUnit> composition)
            : base(database, name, composition)
        {

        }

        /// <summary>
        /// To be called only from <see cref="Database.DefineDerivedUnit(UnitProto)"/>
        /// </summary>
        /// <param name="database"></param>
        /// <param name="name"></param>
        /// <param name="symbol"></param>
        /// <param name="quantity"></param>
        /// <param name="fundamentalMultiplier"></param>
        /// <param name="fundamentalOffset"></param>
        /// <param name="composition"></param>
        internal DerivedUnit(
            Database database,
            string name,
            string? symbol,
            DerivedQuantity quantity,
            decimal fundamentalMultiplier,
            decimal fundamentalOffset,
            NamedComposition<IUnit>? composition)
            : base(database, name, symbol, quantity, fundamentalMultiplier, fundamentalOffset, composition)
        {

        }
    }
}
