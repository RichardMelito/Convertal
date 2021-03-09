using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public interface INamed : IEquatable<INamed>, IComparable<INamed>
    {
        string Name { get; }
        string NameSpace { get; }
        string FullName { get; }
        int HashCode => System.HashCode.Combine(Name, NameSpace, GetType());

        bool IEquatable<INamed>.Equals(INamed? other)
        {
            return ReferenceEquals(this, other);
        }

        int IComparable<INamed>.CompareTo(INamed? other)
        {
            return FullNameComparer.DefaultComparer.Compare(this, other);
        }
    }
}
