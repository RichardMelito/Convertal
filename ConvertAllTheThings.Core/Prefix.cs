using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public record PrefixProto(decimal Multiplier, string Name, string? Symbol) : MaybeNamedProto(Name, Symbol);

    public class Prefix : MaybeNamed, INamed
    {
        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            toIgnore = toIgnore.UnionAppend(this);

            var prefixedUnitsWithThis =
                from prefixedUnit in Database.PrefixedUnits
                where prefixedUnit.Prefix == this
                select prefixedUnit;

            IEnumerable<IMaybeNamed> res = prefixedUnitsWithThis;
            foreach (var prefixedUnit in prefixedUnitsWithThis.Except(toIgnore))
                res = res.Union(prefixedUnit.GetAllDependents(ref toIgnore));

            res.ThrowIfSetContains(this);
            return res.SortByTypeAndName();
        }

        public decimal Multiplier { get; }

        internal Prefix(Database database, string name, decimal multiplier, string? symbol = null)
            : base (database, name, symbol)
        {
            Multiplier = multiplier;
        }

        public PrefixedBaseUnit this[BaseUnit unit]
        {
            get
            {
                return Database.GetPrefixedUnit(unit, this);
            }
        }

        public PrefixedDerivedUnit this[DerivedUnit unit]
        {
            get
            {
                return Database.GetPrefixedUnit(unit, this);
            }
        }

        public PrefixedUnit this[Unit unit]
        {
            get
            {
                return Database.GetPrefixedUnit(unit, this);
            }
        }
    }
}
