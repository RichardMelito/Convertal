// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public interface IVectorOrScalar : IMaybeNamed
{
    bool IsVector { get; }
    bool IsScalar => !IsVector;
}
