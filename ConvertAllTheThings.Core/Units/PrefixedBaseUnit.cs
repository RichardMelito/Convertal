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
        public override BaseUnit Unit { get; }

        public PrefixedBaseUnit(BaseUnit unit, Prefix prefix)
            : base(unit, prefix)
        {
            Unit = unit;
            MaybeBaseUnitComposition = new(this);
        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents()
        {
            var unitsComposedOfThis = IBaseUnit.GetAllIDerivedUnitsComposedOf(this);

            IEnumerable<IMaybeNamed> res = base.GetAllDependents();
            foreach (var unit in unitsComposedOfThis)
                res = res.Union(unit.GetAllDependents());

            return res.SortByTypeAndName();
        }
    }
}
