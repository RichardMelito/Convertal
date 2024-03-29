// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Convertal.Core;

public interface IMaybeNamed : IDisposable, IComparable<IMaybeNamed>, IEquatable<IMaybeNamed>
{
    Database Database { get; }
    string? Name { get; }
    string? Symbol { get; }

    string ToStringSymbol();
    MaybeNamedProto ToProto();

    IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore);

    internal void DisposeThisAndDependents(bool disposeDependents);

    Type GetTypeWithinDictionary() => GetType();

    public T CastAndChangeNameAndSymbol<T>(string name, string? symbol)
        where T : MaybeNamed
    {
        var res = (T)this;
        res.ChangeNameAndSymbol(name, symbol);
        return res;
    }

    int IComparable<IMaybeNamed>.CompareTo(IMaybeNamed? other)
    {
        return MaybeNamed.DefaultComparer.Compare(this, other);
    }

    bool IEquatable<IMaybeNamed>.Equals(IMaybeNamed? other)
    {
        if (ReferenceEquals(this, other))
            return true;

        var type = GetType();
        if (other is null || Database == other.Database || type != other.GetType())
            return false;

        if (type == typeof(EmptyUnit) || type == typeof(EmptyQuantity))
            return true;

        return ToProto() == other.ToProto();
    }

    int CalculateHashCode()
    {
        return HashCode.Combine(Name, GetTypeWithinDictionary());
    }
}
