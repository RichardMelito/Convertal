// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

public interface IVector<TVector, TScalar> : IVectorOrScalar
    where TVector : IVector<TVector, TScalar>
    where TScalar : IScalar<TScalar, TVector>
{
    bool IVectorOrScalar.IsVector => true;

    // TODO make things nullable
    TScalar? ScalarAnalog { get;}

    static virtual TVector operator /(TVector left, TScalar right) => left.Divide(right);

    static virtual TScalar operator *(TVector lhs, TVector rhs) => lhs.DotP(rhs);
    static virtual TVector operator &(TVector lhs, TVector rhs) => lhs.CrossP(rhs);

    TVector Multiply(TScalar scalar);
    TVector Divide(TScalar scalar);

    TScalar DotP(TVector other);
    TVector CrossP(TVector other);
}
