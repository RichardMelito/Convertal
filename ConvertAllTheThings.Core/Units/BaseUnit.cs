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
        /// <summary>
        /// Only to be called from <see cref="BaseQuantity.DefineNewBaseQuantity(string, string, Prefix?)"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="quantity"></param>
        /// <param name="fundamentalMultiplier"></param>
        internal BaseUnit(string name, BaseQuantity quantity, decimal fundamentalMultiplier)
            : base(name, quantity, fundamentalMultiplier)
        {
            MaybeBaseUnitComposition = new(this);
        }

        // for defining from an existng IBaseUnit
        public BaseUnit(string name, IBaseUnit otherUnit, decimal fundamentalMultiplier)
            : base(name, otherUnit, fundamentalMultiplier)
        {
            MaybeBaseUnitComposition = new(this);
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
