
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public class VectorComposition<T> : NamedComposition<T>,
    IVector<VectorComposition<T>, ScalarComposition<T>>
    where T : IMaybeNamed, IVectorOrScalar
{
    public static readonly VectorComposition<T> Empty;

    public ScalarComposition<T> ScalarAnalog
    {
        get
        {
            if (this == Empty)
                return ScalarComposition<T>.Empty;

            return new(MakeAllInDictScalar(this));
        }
    }

    public override bool IsVector => true;

    static VectorComposition()
    {
        Empty = new VectorComposition<T>(new Dictionary<T, decimal>().ToImmutableDictionary());
    }

    internal VectorComposition(IReadOnlyDictionary<T, decimal> composition)
        : base(/*ThrowIfDictInvalid(composition)*/composition)
    {
        
    }

    public VectorComposition(T key) 
        : base(key.IsVector ? key : throw new InvalidOperationException())
    {
        
    }

    //private static IReadOnlyDictionary<T, decimal> ThrowIfDictInvalid(IReadOnlyDictionary<T, decimal> composition)
    //{
    //    var vectorKvps = composition.Where(kvp => kvp.Key.IsVector);
    //    if (!vectorKvps.Any())
    //        throw new InvalidOperationException();

    //    if (vectorKvps.Any(kvp => kvp.Value < 1m))
    //        throw new InvalidOperationException();

    //    return composition;
    //}

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

    public ScalarComposition<T> DotP(VectorComposition<T> other) => ScalarAnalog * other.ScalarAnalog.ReturnIfNotNull();

    public VectorComposition<T> CrossP(VectorComposition<T> other)
        => VectorMultiplyOrDivide(
            ScalarAnalog,
            other.ScalarAnalog.ReturnIfNotNull(),
            true,
            Keys.Concat(other.Keys).Where(key => key.IsVector));
    //{
    //    var resultingComposition = MultiplyOrDivide(ScalarAnalog, other.ScalarAnalog, true);
    //    foreach (var vectorElem in this.Concat(other).Where(kvp => kvp.Key.IsVector))
    //    {
    //        var scalarKey = (T)vectorElem.Key.ToScalar();
    //        if (resultingComposition.TryGetValue(scalarKey, out var resultingPower))
    //        {
    //            if (resultingPower == 1m)
    //            {
    //                resultingComposition.Remove(scalarKey);
    //                resultingComposition.Add(vectorElem.Key, 1m);
    //            }
    //            else if (resultingPower > 1m)
    //            {
    //                resultingComposition[scalarKey] -= 1m;
    //                resultingComposition.Add(vectorElem.Key, 1m);
    //            }
    //        }
    //    }
    //    return new(resultingComposition);
    //}

    public VectorComposition<T> Multiply(ScalarComposition<T> scalar) => scalar * this;
    public VectorComposition<T> Divide(ScalarComposition<T> scalar) => this / scalar;

    public static ScalarComposition<T> operator *(VectorComposition<T> left, VectorComposition<T> right) => left.DotP(right.ReturnIfNotNull());
    public static VectorComposition<T> operator &(VectorComposition<T> left, VectorComposition<T> right) => left.CrossP(right.ReturnIfNotNull());

    public static VectorComposition<T> operator /(VectorComposition<T> vector, ScalarComposition<T> scalar)
        => VectorMultiplyOrDivide(
            vector.ScalarAnalog,
            scalar,
            false,
            vector.Keys.Where(key => key.IsVector));
    //{


    //    SortedDictionary<T, decimal> resultingComposition = new();

    //    var keysInBothSides = scalar.Keys.Intersect(vector.Keys);
    //    foreach (var bothSidesKey in keysInBothSides)
    //    {
    //        var resultingPower = scalar[bothSidesKey] - vector[bothSidesKey];

    //        if (resultingPower != 0.0m)
    //            resultingComposition[bothSidesKey] = resultingPower;
    //    }

    //    var keysInLhs = scalar.Keys.Except(keysInBothSides);
    //    foreach (var lhsKey in keysInLhs)
    //        resultingComposition[lhsKey] = scalar[lhsKey];

    //    var keysInRhs = vector.Keys.Except(keysInBothSides);
    //    foreach (var rhsKey in keysInRhs)
    //        resultingComposition[rhsKey] = -vector[rhsKey];

    //    if (resultingComposition.Count == 0)
    //        return VectorComposition<T>.Empty;

    //    return new VectorComposition<T>(resultingComposition.AsReadOnly());
    //}


    public static VectorComposition<T> CreateFromExistingBaseComposition<TExistingBase>(
        VectorComposition<TExistingBase> existingBaseComposition,
        Func<TExistingBase, T> convertor)

        where TExistingBase : IBase, IComparable<TExistingBase>, IEquatable<TExistingBase>
    {
        SortedDictionary<T, decimal> convertedComposition = new();
        foreach (var (existingBase, power) in existingBaseComposition)
        {
            var convertedBase = convertor(existingBase);
            convertedComposition.Add(convertedBase, power);
        }

        return new VectorComposition<T>(convertedComposition);
    }
}
