using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public abstract class Unit : MaybeNamed, IUnit
    {
        public Quantity Quantity { get; }

        public decimal FundamentalMultiplier { get; }

        protected Unit(string? name, Quantity quantity, decimal fundamentalMultiplier)
            : base(name)
        {
            Quantity = quantity;
            FundamentalMultiplier = fundamentalMultiplier;
        }

        protected Unit(string? name, IUnit otherUnit, decimal multiplier)
            : base(name)
        {
            Quantity = otherUnit.Quantity;
            FundamentalMultiplier = otherUnit.FundamentalMultiplier * multiplier;
        }
    }
}
