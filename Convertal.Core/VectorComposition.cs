
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
    where T : IMaybeNamed
{
    internal VectorComposition(IReadOnlyDictionary<T, decimal> composition)
        : base(composition)
    {

    }

    public VectorComposition(T key)
        : base(key)
    {

    }

    public bool Equals(VectorComposition<T>? other)
    {
        return base.Equals(other as VectorComposition<T>);
    }

    public static VectorComposition<T> operator *(VectorComposition<T> vector, ScalarComposition<T> scalar)
    {
        return new(MultiplyOrDivide(vector, scalar, true));
    }

    public static VectorComposition<T> operator /(VectorComposition<T> vector, ScalarComposition<T> scalar)
    {
        return new(MultiplyOrDivide(vector, scalar, false));
    }

    public static VectorComposition<T> operator %(VectorComposition<T> lhs, VectorComposition<T> rhs)
    {
        return new(MultiplyOrDivide(lhs, rhs, true));
    }

    public static ScalarComposition<T> operator *(VectorComposition<T> lhs, VectorComposition<T> rhs)
    {
        return new(MultiplyOrDivide(lhs, rhs, true));   
    }
}