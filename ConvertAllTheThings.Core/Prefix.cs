using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public class Prefix : MaybeNamed, INamed
    {
        static Prefix()
        {
            AddTypeToDictionary<Prefix>();
        }
        internal static void InitializeClass() { }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            toIgnore = toIgnore.UnionAppend(this);

            var prefixedUnitsWithThis =
                from prefixedUnit in PrefixedUnit.PrefixedUnits
                where prefixedUnit.Prefix == this
                select prefixedUnit;

            IEnumerable<IMaybeNamed> res = prefixedUnitsWithThis;
            foreach (var prefixedUnit in prefixedUnitsWithThis.Except(toIgnore))
                res = res.Union(prefixedUnit.GetAllDependents(ref toIgnore));

            res.ThrowIfSetContains(this);
            return res.SortByTypeAndName();
        }

        public decimal Multiplier { get; }

        public Prefix(string name, decimal multiplier)
            : base (name)
        {
            Multiplier = multiplier;
        }
    }
}
