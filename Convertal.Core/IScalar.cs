// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

public interface IScalar<TScalar, TVector> : IVectorOrScalar
    where TScalar : IScalar<TScalar, TVector>
    where TVector : IVector<TVector, TScalar>
{
    bool IVectorOrScalar.IsVector => false;
    TVector? VectorAnalog { get; }

    static virtual TScalar operator *(TScalar left, TScalar right) => left.Multiply(right);
    static virtual TScalar operator /(TScalar left, TScalar right) => left.Divide(right);

    static virtual TVector operator *(TScalar scalar, TVector vector) => scalar.Multiply(vector);
    static virtual TVector operator *(TVector vector, TScalar scalar) => vector.Multiply(scalar);

    TScalar Multiply(TScalar other);
    TVector Multiply(TVector vector);
    TScalar Divide(TScalar other);

    TScalar Pow(decimal power);

    IVectorOrScalar IVectorOrScalar.ToScalar() => this;
    new TScalar ToScalar() => (TScalar)this;
}
