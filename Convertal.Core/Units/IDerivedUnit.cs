// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public interface IDerivedUnit : IUnit, IDerived
{
}

public interface IDerivedScalarUnit : IDerivedUnit, IScalar<IDerivedScalarUnit, IDerivedVectorUnit>
{
    static abstract IDerivedScalarUnit operator *(IDerivedScalarUnit left, IDerivedScalarUnit right);
}

public interface IDerivedVectorUnit : IDerivedUnit, IVector<IDerivedVectorUnit, IDerivedScalarUnit>
{
}
