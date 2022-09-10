using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Collections.Immutable;

namespace ConvertAllTheThings.Core
{
    public abstract class NamedComposition
    {
        public abstract IReadOnlyDictionary<string, decimal> CompositionAsStringDictionary { get; }
    }

    [JsonConverter(typeof(JsonConverters.NamedCompositionConverter))]
    public class NamedComposition<T> : NamedComposition, IReadOnlyDictionary<T, decimal>, IEquatable<NamedComposition<T>>
        where T : IMaybeNamed
    {
        /*  describes a collection of base quantities or base units from which 
         *  derived quantities or units are formed. 
         */

        public static readonly NamedComposition<T> Empty;

        private readonly ImmutableDictionary<T, decimal> _innerDictionary;

        public IEnumerable<T> Keys => _innerDictionary.Keys;

        public IEnumerable<decimal> Values => _innerDictionary.Values;

        public int Count => _innerDictionary.Count;

        public override IReadOnlyDictionary<string, decimal> CompositionAsStringDictionary => 
            _innerDictionary.ToImmutableDictionary(kvp => kvp.Key.ToString()!, kvp => kvp.Value);

        public decimal this[T key] => _innerDictionary[key];

        static NamedComposition()
        {
            Empty = new NamedComposition<T>(
                new Dictionary<T, decimal>().AsReadOnly());
        }

        internal NamedComposition(IReadOnlyDictionary<T, decimal> composition)
        {
            _innerDictionary = composition.ToImmutableDictionary();
        }

        public NamedComposition(T key)
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
            foreach (var (key, power) in this)
            {
                string powerString;
                if (power == decimal.Truncate(power))
                    powerString = ((int)power).ToString();
                else
                    powerString = power.ToString().TrimEnd('0');

                stringBuilder.Append($"({key.Name!}^{powerString})");
                ++count;
                if (count != (this.Count))
                    stringBuilder.Append('*');
            }

            return stringBuilder.ToString();
        }

        internal static NamedComposition<T> CreateFromExistingBaseComposition<TExistingBase>(
            NamedComposition<TExistingBase> existingBaseComposition,
            Func<TExistingBase, T> convertor)

            where TExistingBase : IBase, IComparable<TExistingBase>, IEquatable<TExistingBase>
        {
            SortedDictionary<T, decimal> convertedComposition = new();
            foreach (var (existingBase, power) in existingBaseComposition)
            {
                var convertedBase = convertor(existingBase);
                convertedComposition.Add(convertedBase, power);
            }

            return new(convertedComposition.AsReadOnly());
        }

        public static NamedComposition<T> MultiplyOrDivide(
            NamedComposition<T> lhs,
            NamedComposition<T> rhs,
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

            if (resultingComposition.Count == 0)
                return Empty;

            return new NamedComposition<T>(resultingComposition.AsReadOnly());
        }

        public static NamedComposition<T> operator *(NamedComposition<T> lhs, NamedComposition<T> rhs)
            => MultiplyOrDivide(lhs, rhs, true);

        public static NamedComposition<T> operator /(NamedComposition<T> lhs, NamedComposition<T> rhs)
            => MultiplyOrDivide(lhs, rhs, false);

        public NamedComposition<T> Pow(decimal power)
        {
            if (power == 0)
                return Empty;

            if (power == 1)
                return this;

            SortedDictionary<T, decimal> newDict = new();
            foreach (var (key, currentPower) in this)
                newDict.Add(key, currentPower * power);

            return new NamedComposition<T>(newDict.AsReadOnly());
        }

        public bool Equals(NamedComposition<T>? other)
        {
            if (other is null)
                return false;

            if (this.Count != other.Count)
                return false;

            // check if any keys in this that are not in other
            // don't need to do the reverse since we already know there 
            // are the same number of keys in each
            if (this.Keys.Except(other.Keys).Any())
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

        public static bool operator == (NamedComposition<T>? lhs, NamedComposition<T>? rhs)
        {
            if (lhs is null)
                return rhs is null;

            return lhs.Equals(rhs);
        }

        public static bool operator != (NamedComposition<T>? lhs, NamedComposition<T>? rhs)
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
    }
}
