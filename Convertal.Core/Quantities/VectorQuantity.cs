// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public abstract class VectorQuantity : Quantity, IVector<VectorQuantity, ScalarQuantity>
{
    // Must be set at construciton by derived types
    protected VectorComposition<IBaseQuantity> SettableBaseQuantityComposition { get; init; } = null!;
    public override VectorComposition<IBaseQuantity> BaseQuantityComposition => SettableBaseQuantityComposition;

    public override bool IsVector => true;

    // TODO needs default generation
    public abstract ScalarQuantity ScalarAnalog { get; }

    protected VectorQuantity(Database database, string? name, string? symbol)
        : base(database, name, symbol)
    {
    }

    public VectorQuantity CrossP(VectorQuantity other)
    {
        var resultingComposition = BaseQuantityComposition.CrossP(other.BaseQuantityComposition);
        return (VectorQuantity)Database.GetFromBaseComposition(resultingComposition);
    }
    public ScalarQuantity DotP(VectorQuantity other)
    {
        var resultingComposition = BaseQuantityComposition.CrossP(other.BaseQuantityComposition);
        return (ScalarQuantity)Database.GetFromBaseComposition(resultingComposition);
    }
}
