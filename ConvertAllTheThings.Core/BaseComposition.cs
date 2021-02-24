using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public class BaseComposition<T> : IEquatable<BaseComposition<T>>
        where T : IBase, IComparable<T>, IEquatable<T>
    {
        /*  describes a collection of base quantities or base units from which 
         *  derived quantities or units are formed. 
         */

        public IReadOnlyDictionary<T, decimal> Composition { get; }

        BaseComposition(IReadOnlyDictionary<T, decimal> composition)
        {
            Composition = composition;
        }

        public BaseComposition(T baseObject)
        {
            Composition = new Dictionary<T, decimal>
                {
                    { baseObject, 1m }
                }.AsReadOnly();
        }

        public static BaseComposition<T> MultiplyOrDivide(
            BaseComposition<T> lhs,
            BaseComposition<T> rhs,
            bool multiplication)
        {
            var multiplyFactor = multiplication ? 1.0m : -1.0m;
            SortedDictionary<T, decimal> resultingComposition = new();
            
            var basesInBothSides = lhs.Composition.Keys.Intersect(rhs.Composition.Keys);
            foreach (var bothSidesBase in basesInBothSides)
            {
                var resultingPower = lhs.Composition[bothSidesBase] + 
                    (multiplyFactor * rhs.Composition[bothSidesBase]);

                if (resultingPower != 0.0m)
                    resultingComposition[bothSidesBase] = resultingPower;
            }

            var basesInLhs = lhs.Composition.Keys.Except(basesInBothSides);
            foreach (var lhsBase in basesInLhs)
                resultingComposition[lhsBase] = lhs.Composition[lhsBase];

            var basesInRhs = rhs.Composition.Keys.Except(basesInBothSides);
            foreach (var rhsBase in basesInRhs)
                resultingComposition[rhsBase] = rhs.Composition[rhsBase] * multiplyFactor);

            return new BaseComposition<T>(resultingComposition.AsReadOnly());
        }

        public static BaseComposition<T> operator *(BaseComposition<T> lhs, BaseComposition<T> rhs)
            => MultiplyOrDivide(lhs, rhs, true);

        public static BaseComposition<T> operator /(BaseComposition<T> lhs, BaseComposition<T> rhs)
            => MultiplyOrDivide(lhs, rhs, false);

        public bool Equals(BaseComposition<T>? other)
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
                var (baseObj, power) = kvp;
                if (other.Composition[baseObj] != power)
                    return false;
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            return Equals(obj as BaseComposition<T>);
        }

        public static bool operator == (BaseComposition<T>? lhs, BaseComposition<T>? rhs)
        {
            if (lhs is null)
                return rhs is null;

            return lhs.Equals(rhs);
        }

        public static bool operator != (BaseComposition<T>? lhs, BaseComposition<T>? rhs)
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
