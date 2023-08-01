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
        var vbqs = defaultDatabase.VectorBaseQuantities;
        Area = (vbqs.Displacement & vbqs.Displacement).CastAndChangeNameAndSymbol<VectorDerivedQuantity>(nameof(Area), "`A");
        Velocity = (vbqs.Displacement / sbqs.Time).CastAndChangeNameAndSymbol<VectorDerivedQuantity>(nameof(Velocity), "`v");
        Acceleration = (Velocity / sbqs.Time).CastAndChangeNameAndSymbol<VectorDerivedQuantity>(nameof(Acceleration), "`a");
        Force = (Acceleration * sbqs.Mass).CastAndChangeNameAndSymbol<VectorDerivedQuantity>(nameof(Force), "`F");
        Torque = (vbqs.Displacement & Force).CastAndChangeNameAndSymbol<VectorDerivedQuantity>(nameof(Torque), "`τ");
        AngularVelocity = (vbqs.AngularDisplacement / sbqs.Time).CastAndChangeNameAndSymbol<VectorDerivedQuantity>(nameof(AngularVelocity), "`ω");
        AngularAcceleration = (AngularVelocity / sbqs.Time).CastAndChangeNameAndSymbol<VectorDerivedQuantity>(nameof(AngularAcceleration), "`α");
    }
}
