// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public interface IVectorOrScalar
{
    bool IsVector { get; }
    bool IsScalar => !IsVector;
    IVectorOrScalar ToScalar();
    IVectorOrScalar? ToVector();
    //static abstract IVectorOrScalar GetEmpty(bool vector);
}
