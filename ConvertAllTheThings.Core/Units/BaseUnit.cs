using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public class BaseUnit : Unit, IBaseUnit
    {

        // for defining from an existng IBaseUnit
        public BaseUnit(
            string name, 
            IBaseUnit otherUnit, 
            decimal multiplier, 
            decimal offset = 0,
            string? symbol = null)
            : base(name, otherUnit, multiplier, offset, symbol)
        {

        }

        /// <summary>
        /// Only to be called from <see cref="BaseQuantity.DefineNewBaseQuantity(string, string, Prefix?)"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="quantity"></param>
        /// <param name="fundamentalMultiplier"></param>
        internal BaseUnit(string name, BaseQuantity quantity, decimal fundamentalMultiplier, string? symbol)
            : base(name, quantity, fundamentalMultiplier, symbol: symbol)
        {

        }


        // for defining from a chain of operations
        internal BaseUnit(string name, NamedComposition<IUnit> composition)
            : base(name, composition)
        {

        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            var res = base.GetAllDependents(ref toIgnore).AsEnumerable();

            var unitsComposedOfThis = IBaseUnit.GetAllIDerivedUnitsComposedOf(this).Cast<IMaybeNamed>();
            res = res.Union(unitsComposedOfThis);

            foreach (var unit in unitsComposedOfThis.Except(toIgnore))
                res = res.Union(unit.GetAllDependents(ref toIgnore));

            res.ThrowIfSetContains(this);
            return res.SortByTypeAndName();
        }
    }
}
