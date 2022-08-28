using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public interface IMaybeNamed : IDisposable, IComparable<IMaybeNamed>, IEquatable<IMaybeNamed>
    {
        string? MaybeName { get; }
        string? MaybeSymbol { get; }

        string ToStringSymbol(); 

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
            return ReferenceEquals(this, other);
        }
        
        int CalculateHashCode()
        {
            return HashCode.Combine(MaybeName, GetTypeWithinDictionary());
        }
    }
}
