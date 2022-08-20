using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public class PrefixedBaseUnit : PrefixedUnit, IBaseUnit
    {
        [JsonConverter(typeof(JsonConverters.ToStringConverter))]
        public new BaseUnit Unit => (BaseUnit)base.Unit;

        internal PrefixedBaseUnit(Database database, BaseUnit unit, Prefix prefix)
            : base(database, unit, prefix)
        {
            
        }

        public IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            var res = IUnit.GetAllDependents(this, ref toIgnore).AsEnumerable();

            var unitsComposedOfThis = Database.GetAllIDerivedUnitsComposedOf(this);
            res = res.Union(unitsComposedOfThis);
            foreach (var unit in unitsComposedOfThis.Except(toIgnore))
                res = res.Union(unit.GetAllDependents(ref toIgnore));

            res.ThrowIfSetContains(this);
            return res.SortByTypeAndName();
        }
    }
}
