using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecimalMath;

namespace ConvertAllTheThings.Core
{
    public abstract class Unit : MaybeNamed, IUnit
    {
        public static EmptyUnit Empty => EmptyUnit.Empty;

        public Quantity Quantity { get; }

        public decimal FundamentalMultiplier { get; }
        public BaseComposition<IBaseUnit>? MaybeBaseUnitComposition { get; protected set; } = null;

        static Unit()
        {
            AddTypeToDictionary<Unit>();
        }

        internal static void InitializeClass() { }

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

        protected Unit(string name, BaseComposition<IBaseUnit> composition)
            : base(name)
        {
            MaybeBaseUnitComposition = composition;
            Quantity = Quantity.GetFromBaseComposition(MaybeBaseUnitComposition);
            FundamentalMultiplier = 1m;
            foreach (var (unit, power) in MaybeBaseUnitComposition.Composition)
            {
                var multiplier = DecimalEx.Pow(unit.FundamentalMultiplier, power);
                FundamentalMultiplier *= multiplier;
            }
        }

        public override IOrderedEnumerable<IUnit> GetAllDependents()
        {

        }

        public override string ToString()
        {
            return MaybeName ?? MaybeBaseUnitComposition!.ToString();
        }

        public bool Equals(IUnit? other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Quantity, FundamentalMultiplier);
        }
    }
}
