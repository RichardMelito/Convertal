using ConvertAllTheThings.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ConvertAllTheThings.Core
{
    public class BaseUnit : Unit, IBaseUnit
    {

        // for defining from an existng IBaseUnit
        internal BaseUnit(
            Database database,
            string name,
            IBaseUnit otherUnit,
            decimal multiplier,
            decimal offset = 0,
            string? symbol = null)
            : base(database, name, otherUnit, multiplier, offset, symbol)
        {

        }

        /// <summary>
        /// Only to be called from <see cref="BaseQuantity.DefineNewBaseQuantity(string, string, Prefix?)"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="quantity"></param>
        /// <param name="fundamentalMultiplier"></param>
        internal BaseUnit(Database database, string name, BaseQuantity quantity, decimal fundamentalMultiplier, string? symbol)
            : base(database, name, quantity, fundamentalMultiplier, symbol: symbol)
        {

        }

        /// <summary>
        /// To be called only from <see cref="Database.DefineBaseUnit(UnitProto)"/>
        /// </summary>
        /// <param name="database"></param>
        /// <param name="name"></param>
        /// <param name="symbol"></param>
        /// <param name="quantity"></param>
        /// <param name="fundamentalMultiplier"></param>
        /// <param name="fundamentalOffset"></param>
        /// <param name="composition"></param>
        internal BaseUnit(
            Database database,
            string name,
            string? symbol,
            BaseQuantity quantity,
            decimal fundamentalMultiplier,
            decimal fundamentalOffset,
            NamedComposition<IUnit>? composition)
            : base(database, name, symbol, quantity, fundamentalMultiplier, fundamentalOffset, composition)
        {

        }


        // for defining from a chain of operations
        internal BaseUnit(Database database, string name, NamedComposition<IUnit> composition)
            : base(database, name, composition)
        {

        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            var res = base.GetAllDependents(ref toIgnore).AsEnumerable();

            var unitsComposedOfThis = Database.GetAllIDerivedUnitsComposedOf(this).Cast<IMaybeNamed>();
            res = res.Union(unitsComposedOfThis);

            foreach (var unit in unitsComposedOfThis.Except(toIgnore))
                res = res.Union(unit.GetAllDependents(ref toIgnore));

            res.ThrowIfSetContains(this);
            return res.SortByTypeAndName();
        }
    }
}
