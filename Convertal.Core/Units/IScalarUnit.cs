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
 *      \ ScalarBaseUnit : IBase
 *      \ ScalarDerivedUnit : IDerived
 *    \ VectorUnit : IVectorUnit
 *      \ VectorBaseUnit : IBase
 *      \ VectorDerivedUnit : IDerived
 *  \ PrefixedUnit
 *    \ ScalarPrefixedUnit : IScalarUnit
 *      \ ScalarPrefixedBaseUnit : IBase
 *      \ ScalarPrefixedDerivedUnit : IDerived
 *    \ VectorPrefixedUnit : IVectorUnit
 *      \ VectorPrefixedBaseUnit : IBase
 *      \ VectorPrefixedDerivedUnit : IDerived
 */

public interface IScalarUnit : IUnit, IScalar<IScalarUnit, IVectorUnit>
{
    // TODO
    static virtual IScalarUnit operator *(IScalarUnit left, IScalarUnit right) => throw new NotImplementedException();
    static virtual IScalarUnit operator /(IScalarUnit left, IScalarUnit right) => throw new NotImplementedException();

    static virtual IVectorUnit operator *(IScalarUnit scalar, IVectorUnit vector) => throw new NotImplementedException();
}
