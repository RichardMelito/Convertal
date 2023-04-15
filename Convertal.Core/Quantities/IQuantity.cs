// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public interface IQuantity : IMaybeNamed, IVectorOrScalar
{
    NamedComposition<IBaseQuantity> BaseQuantityComposition { get; }
    bool Disposed { get; }
    IUnit FundamentalUnit { get; }
    //Quantity MultiplyOrDivide(Quantity lhs, Quantity rhs, bool multiplication);
    string ToString();
}
