using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class PrefixedDerivedUnit : PrefixedUnit, IDerivedUnit
    {
        public override DerivedUnit Unit { get; }

        public PrefixedDerivedUnit(DerivedUnit unit, Prefix prefix) 
            : base(unit, prefix)
        {
            Unit = unit;
        }
    }
}
