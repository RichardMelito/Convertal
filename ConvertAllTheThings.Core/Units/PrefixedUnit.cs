using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public abstract class PrefixedUnit : IUnit, INamed
    {
        public Quantity Quantity => Unit.Quantity;

        public string? MaybeName => Prefix.MaybeName! + "_" + Unit.MaybeName!;

        public decimal FundamentalMultiplier => Unit.FundamentalMultiplier * Prefix.Multiplier;
        public abstract Unit Unit { get; }
        public Prefix Prefix { get; }

        public BaseComposition<IBaseUnit>? MaybeBaseUnitComposition { get; protected set; }

        public override string ToString()
        {
            return MaybeName!;
        }

        protected PrefixedUnit(Unit unit, Prefix prefix)
        {
            if (unit.MaybeName is null)
                throw new ArgumentNullException(
                    nameof(unit), 
                    "Unit in PrefixedUnit must have a name.");

            Prefix = prefix;
        }

        //public Term ConvertTo(IUnit resultingIUnit)
        //{
        //    var unprefixedConversion = Unit.ConvertTo(resultingIUnit);
        //    return unprefixedConversion * Prefix.Multiplier;
        //}

        //public Term ConvertToFundamental()
        //{
        //    var unprefixedFundamental = Unit.ConvertToFundamental();
        //    return unprefixedFundamental * Prefix.Multiplier;
        //}
    }
}
