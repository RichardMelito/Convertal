using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public class PrefixedBaseUnit : PrefixedUnit, IBaseUnit
    {
        public new BaseUnit Unit => (BaseUnit)base.Unit;

        internal PrefixedBaseUnit(BaseUnit unit, Prefix prefix)
            : base(unit, prefix)
        {
            
        }

        public IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            var res = IUnit.GetAllDependents(this, ref toIgnore).AsEnumerable();

            var unitsComposedOfThis = IBaseUnit.GetAllIDerivedUnitsComposedOf(this);
            res = res.Union(unitsComposedOfThis);
            foreach (var unit in unitsComposedOfThis.Except(toIgnore))
                res = res.Union(unit.GetAllDependents(ref toIgnore));

            res.ThrowIfSetContains(this);
            return res.SortByTypeAndName();
        }
    }
}
