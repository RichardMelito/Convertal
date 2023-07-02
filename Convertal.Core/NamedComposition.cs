// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public abstract class NamedComposition<T> : IVectorOrScalar, IReadOnlyDictionary<T, decimal>, IEquatable<NamedComposition<T>>
    where T : IMaybeNamed, IVectorOrScalar
{
    /*  describes a collection of base quantities or base units from which 
     *  derived quantities or units are formed.
     */

    public abstract bool IsVector { get; }

    private readonly IReadOnlyDictionary<T, decimal> _innerDictionary;

    public IEnumerable<T> Keys => _innerDictionary.Keys;

    public IEnumerable<decimal> Values => _innerDictionary.Values;

    public int Count => _innerDictionary.Count;

    public Dictionary<string, decimal> CompositionAsStringDictionary =>
        _innerDictionary.ToDictionary(kvp => kvp.Key.ToString()!, kvp => kvp.Value);

    public decimal this[T key] => _innerDictionary[key];

    internal NamedComposition(IReadOnlyDictionary<T, decimal> composition)
    {
        if (composition.Count > 1 && composition is not SortedDictionary<T, decimal> && composition is not ImmutableSortedDictionary<T, decimal>)
            _innerDictionary = composition.ToImmutableSortedDictionary();
        else
            _innerDictionary = composition.ToImmutableDictionary();
    }

    public static NamedComposition<T> Make(T key)
    {
        if (key.IsVector)
            return new VectorComposition<T>(key);
        else
            return new ScalarComposition<T>(key);
    }

    protected NamedComposition(T key)
    {
        if (key.Name is null)
            throw new ApplicationException();

        _innerDictionary = new Dictionary<T, decimal>
            {
                { key, 1m }
            }.ToImmutableDictionary();
    }

    public bool IsComposedOfOne(T key) => TryGetValue(key, out var power) && power == 1;

    public void ThrowIfRecursive(T keyThatShouldAppearOnceOrNever)
    {
        if (TryGetValue(keyThatShouldAppearOnceOrNever, out var power) && power != 1)
            throw new InvalidOperationException();
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        var count = 0;
        foreach (var (key, power) in this.OrderBy(kvp => kvp.Key))
        {
            string powerString;
            if (power == decimal.Truncate(power))
                powerString = ((int)power).ToString();
            else
                powerString = power.ToString().TrimEnd('0');

            stringBuilder.Append($"({key.Name!}^{powerString})");
            ++count;
            if (count != Count)
                stringBuilder.Append('*');
        }

        return stringBuilder.ToString();
    }

    public static NamedComposition<T> operator *(NamedComposition<T> lhs, NamedComposition<T> rhs)
    {
        if (lhs.IsVector)
        {
            var lhsVec = (VectorComposition<T>)lhs;
            if (rhs.IsVector)
                return lhsVec.DotP((VectorComposition<T>)rhs);

            return ((ScalarComposition<T>)rhs) * lhsVec;
        }
        else
        {
            var lhsScalar = (ScalarComposition<T>)lhs;
            if (rhs.IsVector)
                return lhsScalar * (VectorComposition<T>)rhs;

            return lhsScalar * ((ScalarComposition<T>)rhs);
        }

    }

    public bool Equals(NamedComposition<T>? other)
    {
        if (other?.IsVector != IsVector)
            return false;

        if (Count != other.Count)
            return false;

        // check if any keys in this that are not in other
        // don't need to do the reverse since we already know there 
        // are the same number of keys in each
        if (Keys.Except(other.Keys).Any())
            return false;

        foreach (var kvp in this)
        {
            var (key, power) = kvp;
            if (other[key] != power)
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        return Equals(obj as NamedComposition<T>);
    }

    public static bool operator ==(NamedComposition<T>? lhs, NamedComposition<T>? rhs)
    {
        if (lhs is null)
            return rhs is null;

        return lhs.Equals(rhs);
    }

    public static bool operator !=(NamedComposition<T>? lhs, NamedComposition<T>? rhs)
    {
        return !(lhs == rhs);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        foreach (var kvp in this)
        {
            hashCode.Add(kvp.Key);
            hashCode.Add(kvp.Value);
        }

        return hashCode.ToHashCode();
    }

    public bool ContainsKey(T key)
    {
        return _innerDictionary.ContainsKey(key);
    }

    public bool TryGetValue(T key, [MaybeNullWhen(false)] out decimal value)
    {
        return _innerDictionary.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<T, decimal>> GetEnumerator()
    {
        return _innerDictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_innerDictionary).GetEnumerator();
    }

    protected static IReadOnlyDictionary<T, decimal> MakeAllInDictScalar(
    IReadOnlyDictionary<T, decimal> composition)
    {
        SortedDictionary<T, decimal> res = new();
        foreach (var kvp in composition)
        {
            var scalarKey = (T)kvp.Key.ToScalar();
            if (res.TryGetValue(scalarKey, out var existingValue))
                res[scalarKey] = existingValue + kvp.Value;
            else
                res.Add(scalarKey, kvp.Value);
        }

        return res;
    }

    //protected static IReadOnlyDictionary<T, decimal> MakeAllButOneInDictScalar(
    //    IReadOnlyDictionary<T, decimal> composition)
    //{
    //    var currentVectors = composition.Where(kvp => kvp.Key.IsVector).ToImmutableArray();
    //    var scalars = composition
    //        .Where(kvp => kvp.Key.IsScalar)
    //        .Concat(currentVectors.SkipLast(1));
    //}

    //static IVectorOrScalar IVectorOrScalar.GetEmpty(bool vector) => GetEmpty(vector);

    //public static NamedComposition<T> GetEmpty(bool vector) =>
    //    vector ? VectorComposition<T>.Empty : ScalarComposition<T>.Empty;

    IVectorOrScalar IVectorOrScalar.ToScalar() => ToScalar();
    IVectorOrScalar? IVectorOrScalar.ToVector() => ToVector();

    public ScalarComposition<T> ToScalar()
    {
        if (IsVector)
            return ((VectorComposition<T>)this).ScalarAnalog;

        return (ScalarComposition<T>)this;
    }

    public VectorComposition<T>? ToVector()
    {
        if (IsVector)
            return (VectorComposition<T>)this;

        return ((ScalarComposition<T>)this).VectorAnalog;
    }

    internal static SortedDictionary<T, decimal> MultiplyOrDivide(
        ScalarComposition<T> lhs,
        ScalarComposition<T> rhs,
        bool multiplication)
    {
        var multiplyFactor = multiplication ? 1.0m : -1.0m;
        SortedDictionary<T, decimal> resultingComposition = new();

        var keysInBothSides = lhs.Keys.Intersect(rhs.Keys);
        foreach (var bothSidesKey in keysInBothSides)
        {
            var resultingPower = lhs[bothSidesKey] +
                (multiplyFactor * rhs[bothSidesKey]);

            if (resultingPower != 0.0m)
                resultingComposition[bothSidesKey] = resultingPower;
        }

        var keysInLhs = lhs.Keys.Except(keysInBothSides);
        foreach (var lhsKey in keysInLhs)
            resultingComposition[lhsKey] = lhs[lhsKey];

        var keysInRhs = rhs.Keys.Except(keysInBothSides);
        foreach (var rhsKey in keysInRhs)
            resultingComposition[rhsKey] = rhs[rhsKey] * multiplyFactor;

        return resultingComposition;
    }

    internal static VectorComposition<T> VectorMultiplyOrDivide(
        ScalarComposition<T> lhs,
        ScalarComposition<T> rhs,
        bool multiplication,
        IEnumerable<T> vectorElements)
    {
        ArgumentNullException.ThrowIfNull(lhs);
        ArgumentNullException.ThrowIfNull(rhs);
        ArgumentNullException.ThrowIfNull(vectorElements);

        var resultingComposition = MultiplyOrDivide(lhs, rhs, multiplication);
        if (resultingComposition.Count == 0)
            return VectorComposition<T>.Empty;

        

        foreach (var vectorElem in vectorElements.Distinct())
        {
            var scalarKey = (T)vectorElem.ToScalar();
            if (resultingComposition.TryGetValue(scalarKey, out var resultingPower))
            {
                if (resultingPower == 1m)
                {
                    resultingComposition.Remove(scalarKey);
                    resultingComposition.Add(vectorElem, 1m);
                }
                else if (resultingPower > 1m)
                {
                    resultingComposition[scalarKey] -= 1m;
                    resultingComposition.Add(vectorElem, 1m);
                }
            }
        }

        return new(resultingComposition);
    }
}
