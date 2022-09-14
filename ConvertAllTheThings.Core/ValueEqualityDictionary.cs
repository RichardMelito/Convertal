using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConvertAllTheThings.Core;

public class ValueEqualityDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IEquatable<ValueEqualityDictionary<TKey, TValue>>
    where TKey : notnull
{
    public ValueEqualityDictionary()
    {
    }

    public ValueEqualityDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
    {
    }

    public ValueEqualityDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection)
    {
    }

    public ValueEqualityDictionary(IEqualityComparer<TKey>? comparer) : base(comparer)
    {
    }

    public ValueEqualityDictionary(int capacity) : base(capacity)
    {
    }

    public ValueEqualityDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer) : base(dictionary, comparer)
    {
    }

    public ValueEqualityDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer) : base(collection, comparer)
    {
    }

    public ValueEqualityDictionary(int capacity, IEqualityComparer<TKey>? comparer) : base(capacity, comparer)
    {
    }

    protected ValueEqualityDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public bool Equals(ValueEqualityDictionary<TKey, TValue>? other)
    {
        if (Count != other?.Count)
            return false;

        foreach (var kvp in this)
        {
            if (other.TryGetValue(kvp.Key, out var otherValue))
            {
                if (!EqualityComparer<TValue>.Default.Equals(kvp.Value, otherValue))
                    return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ValueEqualityDictionary<TKey, TValue>);
    }

    public override int GetHashCode()
    {
        // Very bad implementation but I don't think this will ever actually get used.
        return Count;
    }

    public static bool operator ==(ValueEqualityDictionary<TKey, TValue>? lhs, ValueEqualityDictionary<TKey, TValue>? rhs)
    {
        if (lhs is null && rhs is null)
            return true;

        var eq = lhs?.Equals(rhs);
        return eq.HasValue && eq.Value;
    }

    public static bool operator !=(ValueEqualityDictionary<TKey, TValue>? lhs, ValueEqualityDictionary<TKey, TValue>? rhs)
    {
        return !(lhs == rhs);
    }
}
