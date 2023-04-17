// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

public interface IBaseUnit : IUnit, IBase, IEquatable<IBaseUnit>
// TODO interface hierarchy for IEquatable and IComparable
{
    IBaseQuantity BaseQuantity => (IBaseQuantity)Quantity;

    bool IEquatable<IBaseUnit>.Equals(IBaseUnit? other)
    {
        return ReferenceEquals(this, other);
    }
}
