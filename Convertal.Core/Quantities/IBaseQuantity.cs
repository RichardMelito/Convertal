// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

/*  IQuantity
 *   \ IBaseQuantity : IBase
 *   \ IDerivedQuantity : IDerived
 *   \ Quantity
 *     \ ScalarQuantity : IScalar
 *       \ ScalarBaseQuantity : IBaseQuantity
 *       \ ScalarDerivedQuantity : IDerivedQuantity
 *     \ VectorQuantity : IVector
 *       \ VectorBaseQuantity : IBaseQuantity
 *       \ DerivedBaseQuantity : IDerivedQuantity
 */

public interface IBaseQuantity : IQuantity, IBase, IComparable<IBaseQuantity>, IEquatable<IBaseQuantity>
{
    int IComparable<IBaseQuantity>.CompareTo(IBaseQuantity? other) => MaybeNamed.MaybeNameComparer.PerformCompare(this, other);
    bool IEquatable<IBaseQuantity>.Equals(IBaseQuantity? other)
    {
        var castThis = (IMaybeNamed)this;
        var castOther = other as IMaybeNamed;
        return castThis.Equals(castOther);
    }
}
