// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core.Defaults;

public class VectorDerivedQuantities
{
    public readonly VectorDerivedQuantity Area;
    public readonly VectorDerivedQuantity Velocity;
    public readonly VectorDerivedQuantity Acceleration;
    public readonly VectorDerivedQuantity Force;
    public readonly VectorDerivedQuantity Torque;
    public readonly VectorDerivedQuantity AngularVelocity;
    public readonly VectorDerivedQuantity AngularAcceleration;

    public VectorDerivedQuantities(DefaultDatabaseWrapper defaultDatabase)
    {
        var sbqs = defaultDatabase.ScalarBaseQuantities;
        var sdqs = defaultDatabase.ScalarDerivedQuantities;
        var vbqs = defaultDatabase.VectorBaseQuantities;
        Area = (vbqs.Displacement & vbqs.Displacement).CastAndChangeNameAndSymbol<VectorDerivedQuantity>(nameof(Area), "A");
        //Torque.ChangeSymbol("τ");
    }
}
