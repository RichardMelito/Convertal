
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

    public override bool IsVector => true;

    static VectorComposition()
    {
        Empty = new VectorComposition<T>(
            new Dictionary<T, decimal>().ToImmutableDictionary());
    }

    internal VectorComposition(IReadOnlyDictionary<T, decimal> composition)
        : base(composition)
    {

    }

    public VectorComposition(T key)
        : base(key)
    {
        
    }

    public ScalarComposition<T> DotP(VectorComposition<T> other) => throw new NotImplementedException();
    public VectorComposition<T> CrossP(VectorComposition<T> other) => throw new NotImplementedException();
}
