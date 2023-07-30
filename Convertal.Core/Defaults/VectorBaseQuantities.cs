// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core.Defaults;

public class VectorBaseQuantities
{
    public readonly VectorBaseQuantity AngularDisplacement;
    public readonly VectorBaseQuantity Displacement;

    internal VectorBaseQuantities(DefaultDatabaseWrapper defaultDatabase)
    {
        AngularDisplacement = defaultDatabase.Database.GetOrDefineVectorBaseQuantity(
            defaultDatabase.ScalarBaseQuantities.Angle,
            nameof(AngularDisplacement),
            "`Θ");

        Displacement = defaultDatabase.Database.GetOrDefineVectorBaseQuantity(
            defaultDatabase.ScalarBaseQuantities.Length,
            nameof(Displacement),
            "`s");
    }
}
