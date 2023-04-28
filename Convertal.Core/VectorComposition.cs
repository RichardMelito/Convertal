
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public class VectorComposition<T> : NamedComposition<T>,
    IVector<VectorComposition<T>, ScalarComposition<T>>
    where T : IMaybeNamed, IVectorOrScalar
{
    public static readonly VectorComposition<T> Empty;

    public ScalarComposition<T> ScalarAnalog { get; internal set; }

    public override bool IsVector => true;

    static VectorComposition()
    {
        Empty = new VectorComposition<T>(
            new Dictionary<T, decimal>().ToImmutableDictionary())
        {
            ScalarAnalog = ScalarComposition<T>.Empty,
        };
    }

    internal VectorComposition(IReadOnlyDictionary<T, decimal> composition)
        : base(composition)
    {

    }

    public VectorComposition(T key) 
        : base(key)
    {
        
    }

    //public (ScalarComposition<T> ScalarComponent, VectorComposition<T> VectorComponent) GetComponents()
    //{
    //    SortedDictionary<T, decimal> scalarDict = new();
    //    SortedDictionary<T, decimal> vectorDict = new();
    //    foreach (var elem in this)
    //    {
    //        if (elem.Key.IsScalar)
    //            scalarDict.Add(elem.Key, elem.Value);
    //        else
    //            vectorDict.Add(elem.Key, elem.Value);
    //    }

    //    return (new (scalarDict), new(vectorDict));
    //}
    
    public ScalarComposition<T> DotP(VectorComposition<T> other)
    {
        // Is this really sufficient?
        return ScalarAnalog * other.ScalarAnalog;
    }

    public VectorComposition<T> CrossP(VectorComposition<T> other)
    {
        // Just a copy-paste from ScalarComposition
        // Is this really sufficient???
        SortedDictionary<T, decimal> resultingComposition = new();

        var keysInBothSides = Keys.Intersect(other.Keys);
        foreach (var bothSidesKey in keysInBothSides)
        {
            var resultingPower = this[bothSidesKey] + other[bothSidesKey];

            if (resultingPower != 0.0m)
                resultingComposition[bothSidesKey] = resultingPower;
        }

        var keysInthis = Keys.Except(keysInBothSides);
        foreach (var thisKey in keysInthis)
            resultingComposition[thisKey] = this[thisKey];

        var keysInother = other.Keys.Except(keysInBothSides);
        foreach (var otherKey in keysInother)
            resultingComposition[otherKey] = other[otherKey];

        if (resultingComposition.Count == 0)
            return Empty;

        return new VectorComposition<T>(resultingComposition.AsReadOnly());
    }

    // TODO these members really need to be cleaned up, they seem like they could get accidentally recursive
    public VectorComposition<T> Multiply(ScalarComposition<T> scalar) => scalar * this;
    public VectorComposition<T> Divide(ScalarComposition<T> scalar) => this / scalar;

    public static ScalarComposition<T> operator *(VectorComposition<T> left, VectorComposition<T> right) => left.DotP(right);
    public static VectorComposition<T> operator &(VectorComposition<T> left, VectorComposition<T> right) => left.CrossP(right);

    public static VectorComposition<T> operator /(VectorComposition<T> vector, ScalarComposition<T> scalar)
    {
        SortedDictionary<T, decimal> resultingComposition = new();

        var keysInBothSides = scalar.Keys.Intersect(vector.Keys);
        foreach (var bothSidesKey in keysInBothSides)
        {
            var resultingPower = scalar[bothSidesKey] - vector[bothSidesKey];

            if (resultingPower != 0.0m)
                resultingComposition[bothSidesKey] = resultingPower;
        }

        var keysInLhs = scalar.Keys.Except(keysInBothSides);
        foreach (var lhsKey in keysInLhs)
            resultingComposition[lhsKey] = scalar[lhsKey];

        var keysInRhs = vector.Keys.Except(keysInBothSides);
        foreach (var rhsKey in keysInRhs)
            resultingComposition[rhsKey] = -vector[rhsKey];

        if (resultingComposition.Count == 0)
            return VectorComposition<T>.Empty;

        return new VectorComposition<T>(resultingComposition.AsReadOnly());
    }
}
