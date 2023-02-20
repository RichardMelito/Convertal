
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public class ScalarComposition<T> : NamedComposition<T>, 
    IScalar<ScalarComposition<T>, VectorComposition<T>>
    where T : IMaybeNamed
{
    public static readonly ScalarComposition<T> Empty;

    static ScalarComposition()
    {
        Empty = new ScalarComposition<T>(
            new Dictionary<T, decimal>().ToImmutableDictionary());
    }

    
    internal ScalarComposition(IReadOnlyDictionary<T, decimal> composition)
        : base(composition)
    {
    }

    public ScalarComposition(T key)
        : base(key)
    {
    }

    public ScalarComposition<T> Pow(decimal power)
    {
        if (power == 0)
            return Empty;

        if (power == 1)
            return this;

        SortedDictionary<T, decimal> newDict = new();
        foreach (var (key, currentPower) in this)
            newDict.Add(key, currentPower * power);

        return new ScalarComposition<T>(newDict.ToImmutableDictionary());
    }

    public bool Equals(ScalarComposition<T>? other)
    {
        return base.Equals(other as ScalarComposition<T>);
    }

    public static ScalarComposition<T> operator *(ScalarComposition<T> lhs, ScalarComposition<T> rhs)
    {
        return new(MultiplyOrDivide(lhs, rhs, true));
    }

    public static ScalarComposition<T> operator /(ScalarComposition<T> lhs, ScalarComposition<T> rhs)
    {
        return new(MultiplyOrDivide(lhs, rhs, false));
    }

    public static VectorComposition<T> operator *(ScalarComposition<T> scalar, VectorComposition<T> vector)
    {
        return new(MultiplyOrDivide(scalar, vector, true));
    }
}