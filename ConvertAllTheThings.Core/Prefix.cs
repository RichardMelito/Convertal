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

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents()
        {
            var prefixedUnitsWithThis =
                from prefixedUnit in PrefixedUnit.PrefixedUnits
                where prefixedUnit.Prefix == this
                select prefixedUnit;

            IEnumerable<IMaybeNamed> res = prefixedUnitsWithThis;
            foreach (var prefixedUnit in prefixedUnitsWithThis)
                res = res.Union(prefixedUnit.GetAllDependents());

            return res.SortByTypeAndName();
        }

        public decimal Multiplier { get; }

        public Prefix(decimal multiplier, string name)
            : base (name)
        {
            Multiplier = multiplier;
        }
    }
}
