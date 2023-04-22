// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

/* We need compile-time type safety for scalar and vector multiplication operations.
 * 
 * IUnit
 *  \ ScalarUnit : IScalar
 *    \ ScalarBaseUnit : IBase
 *    \ ScalarDerivedUnit : IDerived
 *    \ ScalarPrefixedUnit
 *      \ ScalarPrefixedBaseUnit : IBase
 *      \ ScalarPrefixedDerivedUnit : IDerived
 *  \ VectorUnit : IVector
 *    \ VectorBaseUnit : IBase
 *    \ VectorDerivedUnit : IDerived
 *    \ VectorPrefixedUnit
 *      \ VectorPrefixedBaseUnit : IBase
 *      \ VectorPrefixedDerivedUnit : IDerived
 *    
 * IUnit
 *  \ IScalarUnit : IScalar
 *  \ IVectorUnit : IVector
 *  \ IScalarBaseUnit : IScalarUnit, IBaseUnit
 *  \ IVectorBaseUnit : IVectorUnit, IBaseUnit
 *  \ IScalarDerivedUnit : IScalarUnit, IDerivedUnit
 *  \ IVectorDerivedUnit : IVectorUnit, IDerivedUnit
 *  \ Unit
 *    \ ScalarUnit : IScalarUnit
 *      \ ScalarBaseUnit : IBaseUnit
 *      \ ScalarDerivedUnit : IDerivedUnit
 *    \ VectorUnit : IVectorUnit
 *      \ VectorBaseUnit : IBaseUnit
 *      \ VectorDerivedUnit : IDerivedUnit
 *  \ PrefixedUnit
 *    \ ScalarPrefixedUnit : IScalarUnit
 *      \ ScalarPrefixedBaseUnit : IBaseUnit
 *      \ ScalarPrefixedDerivedUnit : IDerivedUnit
 *    \ VectorPrefixedUnit : IVectorUnit
 *      \ VectorPrefixedBaseUnit : IBaseUnit
 *      \ VectorPrefixedDerivedUnit : IDerivedUnit
 */

public interface IScalarUnit : IUnit, IScalar<IScalarUnit, IVectorUnit>
{
    // TODO
    static virtual IScalarUnit operator *(IScalarUnit left, IScalarUnit right) => throw new NotImplementedException();
    static virtual IScalarUnit operator /(IScalarUnit left, IScalarUnit right) => throw new NotImplementedException();

    static virtual IVectorUnit operator *(IScalarUnit scalar, IVectorUnit vector) => throw new NotImplementedException();

    ScalarTerm ToTerm(decimal magnitude) => new(magnitude, this);
}
