// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Numerics;

namespace Convertal.Core;

public interface IVectorOrScalar : IMaybeNamed
{
    bool IsVector { get; }
    bool IsScalar => !IsVector;
}

public interface IScalar<TScalar, TVector> :
    IAdditionOperators<TScalar, TScalar, TScalar>,
    ISubtractionOperators<TScalar, TScalar, TScalar>,
    IDivisionOperators<TScalar, TScalar, TScalar>,
    IMultiplyOperators<TScalar, TScalar, TScalar>,
    IPowerFunctions<TScalar>,
    IRootFunctions<TScalar>
    where TScalar : IScalar<TScalar, TVector>
    where TVector : IVector<TVector, TScalar>
{
    static abstract TVector operator *(TScalar scalar, TVector vector);
}

public interface IVector<TVector, TScalar> :
    IAdditionOperators<TVector, TVector, TVector>,
    ISubtractionOperators<TVector, TVector, TVector>,

    where TVector : IVector<TVector, TScalar>
    where TScalar : IScalar<TScalar, TVector>
{
}
