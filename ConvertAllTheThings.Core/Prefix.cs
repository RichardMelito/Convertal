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
            toIgnore = toIgnore.Append(this);

            var prefixedUnitsWithThis =
                from prefixedUnit in PrefixedUnit.PrefixedUnits
                where prefixedUnit.Prefix == this
                select prefixedUnit;

            IEnumerable<IMaybeNamed> res = prefixedUnitsWithThis;
            foreach (IUnit prefixedUnit in prefixedUnitsWithThis)
                res = res.Union(prefixedUnit.GetAllDependents());

            res = res.Except(this.AsEnumerable());
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
