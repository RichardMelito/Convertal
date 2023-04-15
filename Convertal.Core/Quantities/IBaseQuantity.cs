// Created by Richard Melito and licensed to you under The Clear BSD License.

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

public interface IBaseQuantity : IQuantity, IBase
{
}
