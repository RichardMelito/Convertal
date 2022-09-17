using System;

namespace ConvertAllTheThings.Core
{
    public interface IBaseUnit : IUnit, IBase, IEquatable<IBaseUnit>
    // TODO interface hierarchy for IEquatable and IComparable
    {
        BaseQuantity BaseQuantity => (BaseQuantity)Quantity;

        bool IEquatable<IBaseUnit>.Equals(IBaseUnit? other)
        {
            return ReferenceEquals(this, other);
        }
    }
}
