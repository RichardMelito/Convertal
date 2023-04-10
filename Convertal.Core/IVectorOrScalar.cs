// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public interface IVectorOrScalar
{
    bool IsVector { get; }
    bool IsScalar => !IsVector;
}

public interface IScalar<TScalar, TVector> : IVectorOrScalar
    where TScalar : IScalar<TScalar, TVector>
    where TVector : IVector<TVector, TScalar>
{
    bool IVectorOrScalar.IsVector => false;
    TVector? VectorAnalog { get; }

    static abstract TScalar operator *(TScalar left, TScalar right);
    static abstract TScalar operator /(TScalar left, TScalar right);

    static abstract TVector operator *(TScalar scalar, TVector vector);
    static virtual TVector operator *(TVector vector, TScalar scalar) => scalar * vector;

    TScalar Pow(decimal power);
}

public interface IVector<TVector, TScalar> : IVectorOrScalar
    where TVector : IVector<TVector, TScalar>
    where TScalar : IScalar<TScalar, TVector>
{
    bool IVectorOrScalar.IsVector => true;
    TScalar ScalarAnalog { get;}

    static virtual TScalar operator *(TVector lhs, TVector rhs) => lhs.DotP(rhs);
    static virtual TVector operator &(TVector lhs, TVector rhs) => lhs.CrossP(rhs);

    TScalar DotP(TVector other);
    TVector CrossP(TVector other);
}
