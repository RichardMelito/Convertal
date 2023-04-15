// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

public interface IScalar<TScalar, TVector> : IVectorOrScalar
    where TScalar : IScalar<TScalar, TVector>
    where TVector : IVector<TVector, TScalar>
{
    bool IVectorOrScalar.IsVector => false;
    TVector? VectorAnalog { get; }

    static virtual TScalar operator *(TScalar left, TScalar right) => throw new NotImplementedException();
    static virtual TScalar operator /(TScalar left, TScalar right) => throw new NotImplementedException();

    static virtual TVector operator *(TScalar scalar, TVector vector) => throw new NotImplementedException();
    static virtual TVector operator *(TVector vector, TScalar scalar) => scalar * vector;

    TScalar Pow(decimal power);
}
