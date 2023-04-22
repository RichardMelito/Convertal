// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public interface IVectorUnit : IUnit, IVector<IVectorUnit, IScalarUnit>
{
    VectorTerm ToTerm(decimal magnitude) => new(magnitude, this);
}

