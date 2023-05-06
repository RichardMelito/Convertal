// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        _innerDictionary = composition;
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

    internal static NamedComposition<T> CreateFromExistingBaseComposition<TExistingBase>(
        NamedComposition<TExistingBase> existingBaseComposition,
        Func<TExistingBase, T> convertor)

        where TExistingBase : IBase, IComparable<TExistingBase>, IEquatable<TExistingBase>
    {
        // TODO might need to store whether the multiplies are dots or crosses in the composition

        SortedDictionary<T, decimal> convertedComposition = new();
        foreach (var (existingBase, power) in existingBaseComposition)
        {
            var convertedBase = convertor(existingBase);
            convertedComposition.Add(convertedBase, power);
        }

        if (existingBaseComposition.IsVector)
            return new VectorComposition<T>(convertedComposition.AsReadOnly());
        else
            return new ScalarComposition<T>(convertedComposition.AsReadOnly());
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
        IReadOnlyDictionary<T, decimal> composition) =>
        composition.ToDictionary(kvp => (T)kvp.Key.ToScalar(), kvp => kvp.Value);

    //protected static IReadOnlyDictionary<T, decimal> MakeAllButOneInDictScalar(
    //    IReadOnlyDictionary<T, decimal> composition)
    //{
    //    var currentVectors = composition.Where(kvp => kvp.Key.IsVector).ToImmutableArray();
    //    var scalars = composition
    //        .Where(kvp => kvp.Key.IsScalar)
    //        .Concat(currentVectors.SkipLast(1));
    //}

    static IVectorOrScalar IVectorOrScalar.GetEmpty(bool vector) => GetEmpty(vector);

    public static NamedComposition<T> GetEmpty(bool vector) =>
        vector ? VectorComposition<T>.Empty : ScalarComposition<T>.Empty;

    public IVectorOrScalar ToScalar() => throw new NotImplementedException();
}
