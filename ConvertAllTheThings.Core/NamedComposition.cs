using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public class NamedComposition<T> : IEquatable<NamedComposition<T>>
        where T : IMaybeNamed, IComparable<T>, IEquatable<T>
    {
        /*  describes a collection of base quantities or base units from which 
         *  derived quantities or units are formed. 
         */

        public static readonly NamedComposition<T> Empty;

        public IReadOnlyDictionary<T, decimal> Composition { get; }

        static NamedComposition()
        {
            Empty = new NamedComposition<T>(
                new Dictionary<T, decimal>().AsReadOnly());
        }

        NamedComposition(IReadOnlyDictionary<T, decimal> composition)
        {
            Composition = composition;
        }


        public NamedComposition(T key)
        {
            if (key.MaybeName is null)
                throw new ApplicationException();

            Composition = new Dictionary<T, decimal>
                {
                    { key, 1m }
                }.AsReadOnly();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            var count = 0;
            foreach (var (key, power) in Composition)
            {
                string powerString;
                if (power == decimal.Truncate(power))
                    powerString = ((int)power).ToString();
                else
                    powerString = power.ToString().TrimEnd('0');

                stringBuilder.Append($"({key.MaybeName!}^{powerString})");
                ++count;
                if (count != (Composition.Count))
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
            foreach (var (existingBase, power) in existingBaseComposition.Composition)
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
            
            var keysInBothSides = lhs.Composition.Keys.Intersect(rhs.Composition.Keys);
            foreach (var bothSidesKey in keysInBothSides)
            {
                var resultingPower = lhs.Composition[bothSidesKey] + 
                    (multiplyFactor * rhs.Composition[bothSidesKey]);

                if (resultingPower != 0.0m)
                    resultingComposition[bothSidesKey] = resultingPower;
            }

            var keysInLhs = lhs.Composition.Keys.Except(keysInBothSides);
            foreach (var lhsKey in keysInLhs)
                resultingComposition[lhsKey] = lhs.Composition[lhsKey];

            var keysInRhs = rhs.Composition.Keys.Except(keysInBothSides);
            foreach (var rhsKey in keysInRhs)
                resultingComposition[rhsKey] = rhs.Composition[rhsKey] * multiplyFactor;

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
            foreach (var (key, currentPower) in Composition)
                newDict.Add(key, currentPower * power);

            return new NamedComposition<T>(newDict.AsReadOnly());
        }

        public bool Equals(NamedComposition<T>? other)
        {
            if (other is null)
                return false;

            if (Composition.Count != other.Composition.Count)
                return false;

            // check if any keys in this that are not in other
            // don't need to do the reverse since we already know there 
            // are the same number of keys in each
            if (Composition.Keys.Except(other.Composition.Keys).Any())
                return false;

            foreach (var kvp in Composition)
            {
                var (key, power) = kvp;
                if (other.Composition[key] != power)
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
            foreach (var kvp in Composition)
            {
                hashCode.Add(kvp.Key);
                hashCode.Add(kvp.Value);
            }

            return hashCode.ToHashCode();
        }
    }
}
