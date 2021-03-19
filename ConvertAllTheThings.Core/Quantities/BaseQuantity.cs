using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class BaseQuantity : Quantity, IBase, IEquatable<BaseQuantity>
    {
        private IBaseUnit? _fundamentalUnit = null;

        public override IBaseUnit FundamentalUnit => _fundamentalUnit!;

        public override BaseComposition<BaseQuantity> BaseQuantityComposition { get; }

        private BaseQuantity(string name)
            : base(name)
        {
            BaseQuantityComposition = new BaseComposition<BaseQuantity>(this);
            Init();
        }

        public static BaseQuantity DefineNewBaseQuantity(
            string quantityName,
            string fundamentalUnitName,
            Prefix? unitPrefix = null)
        {
            // TODO
            ThrowIfNameNotValid<Unit>(fundamentalUnitName);
            ThrowIfNameNotValid<Quantity>(quantityName);


            BaseQuantity quantity = new(quantityName);

            if (unitPrefix is null)
            {
                BaseUnit unit = new(fundamentalUnitName, quantity, 1m);
                quantity._fundamentalUnit = unit;
            }
            else
            {
                var fundamentalMultiplier = 1m / unitPrefix.Multiplier;
                BaseUnit unit = new(fundamentalUnitName, quantity, fundamentalMultiplier);
                quantity._fundamentalUnit = new PrefixedBaseUnit(unit, unitPrefix);
            }

            return quantity;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(BaseQuantity? other)
        {
            return base.Equals(other);
        }
    }
}
